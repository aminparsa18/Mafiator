using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Data.Enums;

namespace Mafiator.Game.Models.Game
{
    public class GameMessageDto : ObservableObject
    {
        public bool Sender { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string DisplayName { get; set; }
        public GameMessageType Type { get; set; }
        private double _totalLength = 100;
        public double TotalLength
        {
            get => _totalLength;
            set => SetProperty(ref _totalLength, value);
        }
        private double _currentPosition;

        public double CurrentPosition
        {
            get => _currentPosition;
            set => SetProperty(ref _currentPosition, value);
        }
        private LayoutState _currentState = LayoutState.Empty;

        public LayoutState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }
        public bool IsPlaying { get; set; }
        public IAsyncRelayCommand PlayVoiceCommand { get; set; }
        public IAsyncRelayCommand PauseVoiceCommand { get; set; }

        public GameMessageDto()
        {
            PlayVoiceCommand = new AsyncRelayCommand(PlayVoice);
            PauseVoiceCommand = new AsyncRelayCommand(PauseVoice);
        }

        private async Task PlayVoice()
        {
            //continue playing same voice
            //if (CrossMediaManager.Current.State == MediaPlayerState.Paused && Content == SystemConstant.PlayingVoice)
            //{
            //    await CrossMediaManager.Current.Play();
            //    CurrentState = LayoutState.Success;

            //}
            //else
            //{
            //    SystemConstant.PlayingVoice = Content;
            //    CurrentState = LayoutState.Loading;
            //    var media = await CrossMediaManager.Current.Play(Content);
            //    IsPlaying = true;
            //    CurrentState = LayoutState.Success;
            //    TotalLength = media.Duration.TotalMilliseconds;
            //}
        }

        private async Task PauseVoice()
        {
           // await CrossMediaManager.Current.Pause();
            CurrentState = LayoutState.Empty;
        }
    }
}