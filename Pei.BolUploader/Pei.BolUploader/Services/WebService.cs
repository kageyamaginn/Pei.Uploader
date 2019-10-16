using Newtonsoft.Json;
using Pei.BolUploader.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Xamarin.Forms;
using SQLite;

namespace Pei.BolUploader.Services
{
    public class WebService
    {
        public static TR Request<TP, TR>(string controller, string action, TP data)
        {
            HttpWebRequest request = WebRequest.Create(String.Format("{0}/{1}/{2}", App.Current.Properties["web_url"], controller, action)) as HttpWebRequest;
            Stream requestStream = request.GetRequestStream();
            using (StreamWriter wr = new StreamWriter(requestStream))
            {
                string dataContent = JsonConvert.SerializeObject(data);
                wr.Write(dataContent);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();

            String responseContent = "";
            using (StreamReader sr = new StreamReader(responseStream))
            {
                responseContent = sr.ReadToEnd();
            }

            try
            {
                if (typeof(TR).IsValueType)
                {
                    MethodInfo miParse = typeof(TR).GetMethod("Parse");
                    if (miParse != null)
                    {
                        return (TR)miParse.Invoke(null, new object[] { responseContent });
                    }

                    else
                    {
                        throw new Exception("Can not analysis result from service");
                    }
                }
                else
                {
                    if (typeof(TR) == typeof(String))
                    {
                        return (TR)(responseContent as object);
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<TR>(responseContent);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            throw new Exception("Response convert not handle");
        }
    }

    public class JobManager
    {
        public static SQLiteConnection jobDb = new SQLiteConnection(Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "jobs.db3"));
        static int WorkCount = 3;
        static List<BackgroundWorker> works;
        static JobManager()
        {
            works = new List<BackgroundWorker>();

        }

        public static void InitializeDb()
        {
            if (jobDb.GetTableInfo("jobs") == null)
            {
                jobDb.CreateTable<BolUploadJob>();
            }

            if (jobDb.GetTableInfo("bolitems") == null)
            {
                jobDb.CreateTable<BolItem>();
            }

            if (jobDb.GetTableInfo("bolimages") == null)
            {
                jobDb.CreateTable<BolImage>();
            }
        }
        private static void CreateWorker()
        {


            BackgroundWorker w = new BackgroundWorker();
            w.WorkerSupportsCancellation = true;
            w.WorkerReportsProgress = true;
            w.DoWork += W_DoWork;
            w.ProgressChanged += W_ProgressChanged;
            w.RunWorkerCompleted += W_RunWorkerCompleted;
            w.RunWorkerAsync(PendingJobs.Dequeue());
            works.Add(w);
        }


        #region worker process
        private static void W_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var job = e.UserState as BolUploadJob;
            // WebService.Request<BolItem,string>()
        }

        private static void W_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            CompletedJobs.Add(e.Result as BolUploadJob);
            works.Remove(sender as BackgroundWorker);
            CacheQueue();
            MessagingCenter.Send<JobManager>(null, "Refresh");
            lock (PendingJobs)
            {
                if (PendingJobs.Count > 0)
                {
                    CreateWorker();
                }
            }

        }

        private static void W_DoWork(object sender, DoWorkEventArgs e)
        {
            CacheQueue();
            MessagingCenter.Send<JobManager>(null, "Refresh");
            var job = e.Argument as BolUploadJob;
            e.Result = job;
            job.Status = "Uploading";
            WorkingJobs.Add(job);
            BolUploadPlan plan = job.GetPlan();
            int retryTimes = 0;
            for (int ai = job.ProcessIndex; ai < plan.Count; ai++)
            {
                if (e.Cancel) { job.Status = "User Cancel"; Cache(job); return; }
                if (plan[ai].Run())
                {
                    retryTimes = 0;
                    Cache(job);
                    plan.Save();
                }
                else
                {
                    if (retryTimes < 3)
                    {
                        ai--;//redo
                        Cache(job);
                        plan.Save();
                    }

                }
            }
            WorkingJobs.Remove(job);

        }

        #endregion

    }
}
