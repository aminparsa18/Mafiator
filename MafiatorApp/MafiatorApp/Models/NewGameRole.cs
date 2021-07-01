using MafiatorApp.Enums;
using MafiatorApp.ViewModels.Base;

namespace MafiatorApp.Models
{
    public class NewGameRole:ExtendedBindableObject
    {
        public GameRole Role { get; set; }
        private bool allowInc;
        public bool AllowInc
        {
            get => allowInc;
            set
            {
                allowInc = value;
                RaisePropertyChanged(() => AllowInc);
            }
        }
        private short _count;
        public short Count
        {
            get => _count;
            set
            {
                _count = value;
                RaisePropertyChanged(() => Count);
            }
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                RaisePropertyChanged(() => Selected);
            }
        }
    }
}
