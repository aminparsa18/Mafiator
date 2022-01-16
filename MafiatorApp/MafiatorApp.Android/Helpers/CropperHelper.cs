using MafiatorApp.Droid.Helpers;
using MafiatorApp.Services;

[assembly: Xamarin.Forms.Dependency(typeof(CropperHelper))]

namespace MafiatorApp.Droid.Helpers
{
    public class CropperHelper:ICropperService
    {
        public void StartCropActivity()
        {
        }
    }
}