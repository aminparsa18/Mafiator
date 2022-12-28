using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Game.Models;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.Extensions.Localization;
using System.Windows.Input;

namespace Mafiator.Game.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private bool playMusic;
    public bool PlayMusic
    {
        get => playMusic;
        set
        {
            _ = TogglePlayMusic();
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
    public IAsyncRelayCommand PlayMusicCommand { get; set; }
    public IRelayCommand AllowNotificationCommand { get; set; }
    public IRelayCommand AutoplayCommand { get; set; }
    public IAsyncRelayCommand ChangeLangCommand { get; set; }
    public IAsyncRelayCommand PopCommand { get; set; }

    private readonly ISubscriber<ChangeLanguageEvent> _subscriber;
    private readonly IDisposable _disposable;

    public SettingsViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
        ISubscriber<ChangeLanguageEvent> subscriber) : base(navigationService, localizer, toastService)
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
            Country = new Country() { Code = culture, Name = "Russian" };
        else
            Country = new Country() { Code = culture, Name = "English" };
        PopCommand = new AsyncRelayCommand(Pop);
        PlaySoundCommand = new Command(TogglePlaySound);
        PlayMusicCommand = new AsyncRelayCommand(TogglePlayMusic);
        AllowNotificationCommand = new RelayCommand(ToggleAllowNotification);
        AutoplayCommand = new RelayCommand(ToggleAutoplay);
        ChangeLangCommand = new AsyncRelayCommand(ChangeLang);
    }

    private void LangChanged()
    {
        var culture = Barrel.Current.Get<string>("Culture");
        if (culture == "RU")
            Country = new Country() { Code = culture, Name = "Russian" };
        else
            Country = new Country() { Code = culture, Name = "English" };
    }

    private async Task ChangeLang() => await _navigationService.NavigateToPopupAsync<LanguagesViewModel>();

    private void ToggleAutoplay() => Autoplay = !Autoplay;

    private async Task TogglePlayMusic()
    {
        if (Initial) return;
        //make it reverse
        //if (!PlayMusic)
        //{
        //    if (CrossMediaManager.Current.IsPrepared())
        //        await CrossMediaManager.Current.Play();
        //    else
        //        await CrossMediaManager.Current.PlayFromAssembly("mafia1.mp3", Assembly.GetExecutingAssembly());
        //}
        //else
        //{
        //    if (CrossMediaManager.Current.IsPlaying())
        //        await CrossMediaManager.Current.Pause();
        //}
    }

    private void ToggleAllowNotification() => AllowNotification = !AllowNotification;

    private void TogglePlaySound() => PlaySound = !PlaySound;

    private async Task Pop()
    {
        Barrel.Current.Add("PlayMusic", PlayMusic, TimeSpan.FromDays(200));
        Barrel.Current.Add("PlaySound", PlaySound, TimeSpan.FromDays(200));
        Barrel.Current.Add("AllowNotification", AllowNotification, TimeSpan.FromDays(200));
        Barrel.Current.Add("Autoplay", Autoplay, TimeSpan.FromDays(200));
        _disposable.Dispose();
        await _navigationService.RemovePopupAsync();
    }
}