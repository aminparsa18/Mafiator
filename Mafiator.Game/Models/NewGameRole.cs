using CommunityToolkit.Mvvm.ComponentModel;
using Mafiator.Common.Data.Enums;

namespace Mafiator.Game.Models
{
    public class NewGameRole : ObservableObject
    {
        public GameRole Role { get; set; }

        private bool allowInc;
        public bool AllowInc
        {
            get => allowInc;
            set => SetProperty(ref allowInc, value);
        }
        private short _count;
        public short Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }
    }
}