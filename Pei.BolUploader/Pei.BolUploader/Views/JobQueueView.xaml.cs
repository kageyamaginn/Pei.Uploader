using Pei.BolUploader.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pei.BolUploader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobQueueView : ContentView
    {
        private JobQueueViewModel Model { get; set; }
        public JobQueueView()
        {
            InitializeComponent();
            Model = new JobQueueViewModel();
            this.BindingContext = Model;
            
        }


    }
}