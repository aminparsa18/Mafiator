namespace Mafiator.Game.Views;

public partial class BarcodeScannerView : ContentPage
	{
		public BarcodeScannerView()
		{
			InitializeComponent();
		}

    private void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var value = e.Results;
    }

}