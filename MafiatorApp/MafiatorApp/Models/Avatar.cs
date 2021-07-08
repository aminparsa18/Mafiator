using Xamarin.CommunityToolkit.ObjectModel;

namespace MafiatorApp.Models
{
    public class Avatar: ObservableObject
    {
        public string Name { get; set; }
        private double scale=1;
        public double Scale
        {
            get => scale;
            set => SetProperty(ref scale, value);
        }
    }
}
