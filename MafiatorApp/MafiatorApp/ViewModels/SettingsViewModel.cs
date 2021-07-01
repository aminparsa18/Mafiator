using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MafiatorApp.Cache;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class SettingsViewModel:ViewModelBase
    {
        private bool playMusic;
        public bool PlayMusic
        {
            get => playMusic;
            set
            {
                playMusic = value;
                RaisePropertyChanged(()=>PlayMusic);
            }
        }

        private bool playSound;
        public bool PlaySound
        {
            get => playSound;
            set
            {
                playSound = value;
                RaisePropertyChanged(() => PlaySound);
            }
        }

        private bool allowNotification;
        public bool AllowNotification
        {
            get => allowNotification;
            set
            {
                allowNotification = value;
                RaisePropertyChanged(() => AllowNotification);
            }
        }

        private bool autoPlay;
        public bool Autoplay
        {
            get => autoPlay;
            set
            {
                autoPlay = value;
                RaisePropertyChanged(() => Autoplay);
            }
        }

        public ICommand PlaySoundCommand { get; set; }
        public ICommand PlayMusicCommand { get; set; }
        public ICommand AllowNotificationCommand { get; set; }
        public ICommand AutoplayCommand { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        public SettingsViewModel()
        {
            PlayMusic = !Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic") ;
            PlaySound = !Barrel.Current.Exists("PlaySound") || Barrel.Current.Get<bool>("PlaySound") ;
            AllowNotification = !Barrel.Current.Exists("AllowNotification") || Barrel.Current.Get<bool>("AllowNotification") ;
            Autoplay = !Barrel.Current.Exists("Autoplay") || Barrel.Current.Get<bool>("Autoplay") ;
            PopCommand=new AsyncCommand(Pop);
            PlaySoundCommand=new Command(TogglePlaySound);
            PlayMusicCommand=new Command(TogglePlayMusic);
            AllowNotificationCommand=new Command(ToggleAllowNotification);
            AutoplayCommand=new Command(ToggleAutoplay);
        }

        private void ToggleAutoplay()
        {
            Autoplay = !Autoplay;
        }

        private void TogglePlayMusic()
        {
            PlayMusic = !PlayMusic;
            if (PlayMusic)
            {
                if (CrossSimpleAudioPlayer.Current.Load("mafia1.mp3"))
                    CrossSimpleAudioPlayer.Current.Play();
            }
            else
            {
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
           Barrel.Current.Add("PlayMusic",PlayMusic,TimeSpan.FromDays(200));
           Barrel.Current.Add("PlaySound", PlaySound,TimeSpan.FromDays(200));
           Barrel.Current.Add("AllowNotification", AllowNotification,TimeSpan.FromDays(200));
           Barrel.Current.Add("Autoplay", Autoplay ,TimeSpan.FromDays(200));
           await NavigationService.RemovePopupAsync();
        }
    }
}
