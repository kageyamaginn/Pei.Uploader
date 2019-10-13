using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Pei.BolUploader.Services;
using Pei.BolUploader.Views;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;

namespace Pei.BolUploader
{
    public partial class App : Application
    {

        public App()
        {

            InitializeComponent();
            // Must be set before you use the picker for the first time.
            ScanditService.ScanditLicense.AppKey = "AYBtgDgnJsx6DFps/jgz/cIdIvvrMIv3nQMD9o0agfHLfd7seVzRf00TlzbgeMJmKxTNN21u0OGCSu1Zc0vcXsFJMGdYbc/TATItCylNNzEycaxE0WTOdc5oiKyqBco/zCTsT4URRJNlE6ickIyanGrCuPDaxD2I8/OAXMZ9GqZHyJ4qppCFYmgz2eBfb15SWi4Dv1VzVmwHG9EYOnOHr8cB6aF8gjVGpgQf3nIeBP60oUKNP2ueIfLX973NRpUOR8UisUs7oB4DRS852zw9twtQ1tMtcRuQcfirsSTV0ads1Ow7QEvi6Nh7pKet/+pwxSZYlvYmZz/KyuhLbDhd09+RYhGvtH7OHELFzHwTMyrZOavizq0RQGwnButSGoKgmU9eMr4kYw8K7CRxrHHDAGPhRRbII7CO52Hxgs1DDC3VgJtJTo5WBEN6hcldLdeQkV8Z3zvPyohfVMrMprImj/9jufUfjbyzmzG+fRYxPXrciNG+IqaEY61lIJowi+QW7Lqdb7rflYRR3dQabqQIGg//rKDChjXVc3C89rtxPX3QsIT+qyLJ+zv72JqAqZx0to8V+IqW+Nh49UvaZkf7GYhCZqNEndiHWLKjH+F3jAW9JvHOx/3px8nNBmaFWueUPYsJtAioO1xjZi5MdzgsHv6dv+2RoXzMSUgytF18FwUee7LFFr1RWdtr20xEjnBCuuzrRig9ANzzPWq/d9qqi3M/3gfXd5mPp0Jbn0QCVxoPj6KPiWuhQ7Q0aSHkAcmK7EMFoioyjkQAd31dz2VWJuQmp48xOpeOIhFJfcgKSNTx1zsJQ/EBbQ==";
      
            MainPage = new WelPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
       
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
