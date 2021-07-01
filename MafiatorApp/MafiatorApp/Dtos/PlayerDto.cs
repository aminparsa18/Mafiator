using MafiatorApp.Enums;
using MafiatorApp.ViewModels.Base;

namespace MafiatorApp.Dtos
{
    public class PlayerDto:ExtendedBindableObject
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public int Score { get; set; }
        private PlayerStatus _status;
        public PlayerStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(()=>Status);
            }
        }
    }
}
