using Pei.BolUploader.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pei.BolUploader.Services
{
    public class WebService
    {
    }

    public class JobManager
    {
        static Queue<BolUploadJob> jobs = new Queue<BolUploadJob>();
        public static void Start(BolUploadJob job)
        {
            jobs.Enqueue(job);
        }
    }
}
