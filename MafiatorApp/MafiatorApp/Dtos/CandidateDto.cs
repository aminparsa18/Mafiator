using MafiatorApp.ViewModels.Base;

namespace MafiatorApp.Dtos
{
    public class CandidateDto:ExtendedBindableObject
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        private bool selected;

        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                RaisePropertyChanged(()=>Selected);
            }
        }
    }
}
