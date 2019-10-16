using Newtonsoft.Json;
using Pei.BolUploader.Entities;
using Pei.BolUploader.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Pei.BolUploader
{
    public delegate bool UploadPlanAction(String key);
    public static class BolUploadJobExt
    {
        public static BolItem GetItem(this BolUploadJob job)
        {
            List< BolItem> items= JobManager.jobDb.Query<BolItem>("select * from bolitems where bolnumber=?", job.BusinessKey);
            if (items.Count == 1)
            {
                return items[0];
            }
            return null;
        }

        public static List<BolImage> GetImages(this BolUploadJob job)
        {
            List<BolImage> items = JobManager.jobDb.Query<BolImage>("select * from BolImage where bolnumber=?", job.BusinessKey);
            return items;
        }
        public static BolUploadPlan GetPlan(this BolUploadJob job)
        {
            BolUploadPlan actions = new BolUploadPlan(job.Data.BolNumber);
            actions.Add(new BolUploadPlanItem {
                Action=UploadItem,
                Key=job.Data.BolNumber
            });

            //foreach (BolImage img in job.Data.Images)
            //{
            //    actions.Add(new BolUploadPlanItem
            //    {
            //        Action = UploadImage,
            //        Key = String.Format("{0}, {1}", img.BolNumber, img.Order)
            //    }); ;
            //}
            actions.Save();
            return actions;
        }
        

        public static bool UploadItem(String key)
        {
            string result =  WebService.Request<BolItem, string>("Bol", "UploadItem", JsonConvert.DeserializeObject<BolItem>(Application.Current.Properties[key].ToString()));
            if (result == "OK")
            {
                return true;
            }
            return false;
        }
        public static bool UploadImage(string key)
        {
            string result = WebService.Request<BolImage, string>("Bol", "UploadImage", JsonConvert.DeserializeObject<BolImage>(Application.Current.Properties[key].ToString()));
            if (result == "OK")
            {
                return true;
            }
            return false;
        }
    }

    public class BolUploadPlanItem
    {
        public UploadPlanAction Action { get; set; }
        public String Key { get; set; }
        public bool Run()
        {
            Successed= Action.Invoke(Key);
            return Successed;
        }

        public bool Successed { get; set; }
    }

    public class BolUploadPlan:List<BolUploadPlanItem>
    {
        public BolUploadPlan(string bolnumber)
        {
            this.BolNumber = bolnumber;
        }
        public string BolNumber { get; set; }
        public async void Save()
        {
            if (Application.Current.Properties.ContainsKey(BolNumber))
            {
                Application.Current.Properties[BolNumber+"_plan"] = JsonConvert.SerializeObject(this);
                await Application.Current.SavePropertiesAsync();
            }
            else
            {
                Application.Current.Properties.Add(BolNumber + "_plan", JsonConvert.SerializeObject(this));
            }
            
        }
        public BolUploadPlan Get()
        {
            string content = Application.Current.Properties[BolNumber + "_plan"].ToString();
            return JsonConvert.DeserializeObject<BolUploadPlan>(content);
        }
    }
}
