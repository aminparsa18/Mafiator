using MafiatorApp.ViewModels.Base;

namespace MafiatorApp.Models
{
    public class Avatar: ExtendedBindableObject
    {
        public string Name { get; set; }
        private double scale=1;
        public double Scale
        {
            get => scale;
            set
            {
                scale = value;
                RaisePropertyChanged(() => Scale);
            }
        }
    }
}
