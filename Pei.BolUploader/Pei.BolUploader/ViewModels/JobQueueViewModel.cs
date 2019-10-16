using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Pei.BolUploader.Entities;
using Pei.BolUploader.Services;

namespace Pei.BolUploader.ViewModels
{
    public class JobQueueViewModel : INotifyPropertyChanged
    {
        public JobQueueViewModel()
        {
            JobQueue = new ObservableCollection<BolUploadJob>();
            MessagingCenter.Subscribe<JobManager>(this,"Refresh",(obj)=> {
               
            });
        }
        private ObservableCollection<BolUploadJob> jobQueue;

        public ObservableCollection<BolUploadJob> JobQueue
        {
            get { return jobQueue; }
            set
            {
                jobQueue = value;
                FirePropertyChange("JobQueue");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void FirePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
