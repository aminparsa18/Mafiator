using System;
using GoogleVisionBarCodeScanner;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BarcodeScannerView : ContentPage
	{
		public BarcodeScannerView()
		{
			InitializeComponent();
		}

        private void CameraView_OnOnDetected(object sender, OnDetectedEventArg e)
        {
            var obj = e.BarcodeResults;

            var result = string.Empty;
            for (var i = 0; i < obj.Count; i++)
            {
                result += $"{i + 1}. Type : {obj[i].BarcodeType}, Value : {obj[i].DisplayValue}{Environment.NewLine}";
            }
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Result", result, "OK");
                //If you want to stop scanning, you can close the scanning page
                await Navigation.PopModalAsync();
            });
        }
    }
}