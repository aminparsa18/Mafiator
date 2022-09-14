using Mafiator.Common.Data.Enums;
using Xamarin.CommunityToolkit.ObjectModel;

namespace MafiatorApp.Dtos.Game
{
    public class PlayerDetails : ObservableObject
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public int Score { get; set; }
        private PlayerStatus _status;
        public PlayerStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
    }
}