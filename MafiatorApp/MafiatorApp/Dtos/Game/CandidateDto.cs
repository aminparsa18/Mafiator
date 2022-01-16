using Xamarin.CommunityToolkit.ObjectModel;

namespace MafiatorApp.Dtos.Game
{
    public class CandidateDto:ObservableObject
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        private bool selected;

        public bool Selected
        {
            get => selected;
            set => SetProperty(ref selected, value);
        }
    }
}
