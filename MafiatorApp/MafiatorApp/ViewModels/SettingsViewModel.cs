using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MafiatorApp.Cache;
using MafiatorApp.Models;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using Plugin.SimpleAudioPlayer;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool playMusic;

        public bool PlayMusic
        {
            get => playMusic;
            set
            {
                TogglePlayMusic();
                SetProperty(ref playMusic, value);
            }
        }

        private bool playSound;

        public bool PlaySound
        {
            get => playSound;
            set => SetProperty(ref playSound, value);
        }

        private bool allowNotification;

        public bool AllowNotification
        {
            get => allowNotification;
            set => SetProperty(ref allowNotification, value);
        }

        private bool autoPlay;

        public bool Autoplay
        {
            get => autoPlay;
            set => SetProperty(ref autoPlay, value);
        }

        private Country country;

        public Country Country
        {
            get => country;
            set => SetProperty(ref country, value);
        }

        public bool Initial = true;
        public ICommand PlaySoundCommand { get; set; }
        public ICommand PlayMusicCommand { get; set; }
        public ICommand AllowNotificationCommand { get; set; }
        public ICommand AutoplayCommand { get; set; }
        public IAsyncCommand ChangeLangCommand { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        private readonly ISubscriber<ChangeLanguageEvent> _subscriber;
        private readonly IDisposable _disposable;

        public SettingsViewModel(ISubscriber<ChangeLanguageEvent> subscriber)
        {
            _subscriber = subscriber;
            var bag = DisposableBag.CreateBuilder();
            _subscriber.Subscribe(c => LangChanged()).AddTo(bag);
            _disposable = bag.Build();
            PlayMusic = !Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic");
            PlaySound = !Barrel.Current.Exists("PlaySound") || Barrel.Current.Get<bool>("PlaySound");
            AllowNotification = !Barrel.Current.Exists("AllowNotification") ||
                                Barrel.Current.Get<bool>("AllowNotification");
            Autoplay = !Barrel.Current.Exists("Autoplay") || Barrel.Current.Get<bool>("Autoplay");
            var culture = Barrel.Current.Get<string>("Culture");
            if (culture == "RU")
                Country = new Country() {Code = culture, Name = "Russian"};
            else
                Country = new Country() {Code = culture, Name = "English"};
            PopCommand = new AsyncCommand(Pop);
            PlaySoundCommand = new Command(TogglePlaySound);
            PlayMusicCommand = new Command(TogglePlayMusic);
            AllowNotificationCommand = new Command(ToggleAllowNotification);
            AutoplayCommand = new Command(ToggleAutoplay);
            ChangeLangCommand = new AsyncCommand(ChangeLang);

        }

        private void LangChanged()
        {
            var culture = Barrel.Current.Get<string>("Culture");
            if (culture == "RU")
                Country = new Country() {Code = culture, Name = "Russian"};
            else
                Country = new Country() {Code = culture, Name = "English"};
        }

        private async Task ChangeLang()
        {
            await NavigationService.NavigateToPopupAsync<LanguagesViewModel>();
        }

        private void ToggleAutoplay()
        {
            Autoplay = !Autoplay;
        }

        private void TogglePlayMusic()
        {
            if (Initial) return;
            //make it reverse
            if (!PlayMusic)
            {
                if (CrossSimpleAudioPlayer.Current.Load("mafia1.mp3"))
                    CrossSimpleAudioPlayer.Current.Play();
            }
            else
            {
                if (CrossSimpleAudioPlayer.Current.IsPlaying)
                    CrossSimpleAudioPlayer.Current.Pause();
            }

        }

        private void ToggleAllowNotification()
        {
            AllowNotification = !AllowNotification;
        }

        private void TogglePlaySound()
        {
            PlaySound = !PlaySound;
        }

        private async Task Pop()
        {
            Barrel.Current.Add("PlayMusic", PlayMusic, TimeSpan.FromDays(200));
            Barrel.Current.Add("PlaySound", PlaySound, TimeSpan.FromDays(200));
            Barrel.Current.Add("AllowNotification", AllowNotification, TimeSpan.FromDays(200));
            Barrel.Current.Add("Autoplay", Autoplay, TimeSpan.FromDays(200));
            _disposable.Dispose();
            await NavigationService.RemovePopupAsync();
        }
    }
}