using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using Pei.BolUploader.Entities;
using Pei.BolUploader.Services;

namespace Pei.BolUploader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanView : ContentView
    {
        public ViewMode vm { get; set; }
        public ScanView()
        {
            InitializeComponent();
            this.BindingContext = vm = new ViewMode() ;
        }
        public string BolNumber { get; set; }
        public async void BeginScan()
        {
            
            // Configure the barcode picker through a scan settings instance by defining which
            // symbologies should be enabled.
            var settings = ScanditService.BarcodePicker.GetDefaultScanSettings();
            // prefer backward facing camera over front-facing cameras.
            settings.CameraPositionPreference = CameraPosition.Back;
            
            //settings.ActiveScanningAreaPortrait = new Rect(0.05, 0.45, 0.9, 0.1);
            ScanditService.BarcodePicker.ScanOverlay.GuiStyle = GuiStyle.Rectangle;
            // Enable symbologies that you want to scan.
            foreach (Symbology s in Enum.GetValues(typeof(Symbology)))
            {
                settings.EnableSymbology(s, true);
            }
            //settings.EnableSymbology(Symbology.Unknown, true);
            //settings.EnableSymbology(Symbology.Upca, true);
            //settings.CodeRejectionEnabled = false;
            //settings.EnableSymbology(Symbology.Qr, true);
            //settings.MaxNumberOfCodesPerFrame = 1;
            
            ScanditService.BarcodePicker.DidScan += OnDidScan;
            await ScanditService.BarcodePicker.ApplySettingsAsync(settings);
            // Start the scanning process.
            await ScanditService.BarcodePicker.StartScanningAsync();

            //cont.Children.Add(ScanditService.BarcodePicker as View);
        }

        private void OnDidScan(ScanSession session)
        {
            Device.BeginInvokeOnMainThread(() => {
                vm.SetBolNumber ( session.AllRecognizedCodes.First().Data);
              
                ScanditService.BarcodePicker.DidScan -= OnDidScan;
                uploadDate.Date = DateTime.Now;
            });
            session.StopScanning();
           
           // this.Navigation.PopAsync();
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            var ps = Application.Current.Properties;
            (this.Parent as ContentPage).DisplayAlert("title", ps.Count.ToString(), "ok");
            BeginScan();
        }
        
        private async void TakePhoto(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(vm.item.BolNumber))
            {
                var page = this.Parent as ContentPage;
                await page.DisplayAlert("Err","Please scan the Bol Number first","I Know");
                return;
            }

            var ph = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { RotateImage = true ,AllowCropping=true}) ;
            byte[] buffer = null;
            Stream imageStream = ph.GetStream();
            using (BinaryReader sr = new BinaryReader(imageStream))
            {
                buffer = new byte[imageStream.Length];
                sr.Read(buffer, 0, (int)imageStream.Length);
            }
            BolImage bolImg = new BolImage { BolNumber = vm.item.BolNumber, Order = vm.item.Images.Count - 1, Data = Convert.ToBase64String(buffer) };
            StackLayout imageContainer = new StackLayout();
            imageContainer.Orientation = StackOrientation.Vertical;
            Image imgView = new Image();
            imgView.HeightRequest = 300;
            imgView.WidthRequest = 300;
            imgView.Source = ImageSource.FromStream(()=>ph.GetStream());
            Button delbtn = new Button();
            delbtn.Text = "Remove";
            delbtn.BindingContext = bolImg;
            delbtn.Clicked += Delbtn_Clicked;
            imageContainer.Children.Add(imgView);
            imageContainer.Children.Add(delbtn);
            ImageView.Children.Add(imageContainer);
            
            vm.item.Images.Add(bolImg);

        }

        private void Delbtn_Clicked(object sender, EventArgs e)
        {
            int imageIndex = vm.item.Images.IndexOf((sender as Button).BindingContext as BolImage);
            vm.item.Images.Remove((sender as Button).BindingContext as BolImage);
            ImageView.Children.Remove((sender as Button).Parent as View);

        }

        private void Reset(object sender, EventArgs e)
        {
            vm.SetBolNumber("");
            vm.item.Images.Clear();
            ImageView.Children.Clear();

        }

        private void btnUpload_Clicked(object sender, EventArgs e)
        {
            JobManager.Start(new BolUploadJob() { JobId = Guid.NewGuid(),Data=vm.item, CreateDate=DateTime.Now, ProcessIndex=0, Status="Pending" });
        }
    }
    public class ViewMode:INotifyPropertyChanged
    {
        public ViewMode() { item = new BolItem(); }
        public BolItem item { get; set; }
        public void SetBolNumber(string value)
        {
            item.BolNumber = value;
            FireChange("item");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void FireChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}