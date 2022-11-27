using CommunityToolkit.Mvvm.ComponentModel;

namespace Mafiator.Game.Models
{
    public class Avatar : ObservableObject
    {
        public string Name { get; set; }

        private double _scale = 1;
        public double Scale
        {
            get => _scale;
            set => SetProperty(ref _scale, value);
        }
    }
}