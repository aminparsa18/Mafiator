using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Data.Dtos.Countries;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using System.Windows.Input;

namespace Mafiator.Game.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ISubscriber<ChangeLanguageEvent> _subscriber;
    private readonly IDisposable _disposable;

    [ObservableProperty]
    private bool _initial;

    [ObservableProperty]
    private bool playMusic;

    [ObservableProperty]
    private bool playSound;

    [ObservableProperty]
    private bool allowNotification;

    [ObservableProperty]
    private bool autoPlay;

    [ObservableProperty]
    private CountryResult country;

    public ICommand PlaySoundCommand { get; set; }
    public IAsyncRelayCommand PlayMusicCommand { get; set; }
    public IRelayCommand AllowNotificationCommand { get; set; }
    public IRelayCommand AutoplayCommand { get; set; }
    public IAsyncRelayCommand ChangeLangCommand { get; set; }
    public IAsyncRelayCommand PopCommand { get; set; }

    public SettingsViewModel(INavigationService navigationService, IToastService toastService, 
        ISubscriber<ChangeLanguageEvent> subscriber) : base(navigationService, toastService)
    {
        _subscriber = subscriber;
        var bag = DisposableBag.CreateBuilder();
        _subscriber.Subscribe(c => LangChanged()).AddTo(bag);
        _disposable = bag.Build();
        PlayMusic = !Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic");
        PlaySound = !Barrel.Current.Exists("PlaySound") || Barrel.Current.Get<bool>("PlaySound");
        AllowNotification = !Barrel.Current.Exists("AllowNotification") ||
                            Barrel.Current.Get<bool>("AllowNotification");
        AutoPlay = !Barrel.Current.Exists("Autoplay") || Barrel.Current.Get<bool>("Autoplay");
        var culture = Barrel.Current.Get<string>("Culture");
        if (culture == "RU")
            Country = new CountryResult() { Sign = culture, Name = "Russian" };
        else
            Country = new CountryResult() { Sign = culture, Name = "English" };
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
            Country = new CountryResult() { Sign = culture, Name = "Russian" };
        else
            Country = new CountryResult() { Sign = culture, Name = "English" };
    }

    private async Task ChangeLang() => await _navigationService.NavigateToPopupAsync<LanguagesViewModel>();

    private void ToggleAutoplay() => AutoPlay = !AutoPlay;

    private async Task TogglePlayMusic()
    {
        if (_initial) return;
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
        Barrel.Current.Add("Autoplay", AutoPlay, TimeSpan.FromDays(200));
        _disposable.Dispose();
        await _navigationService.RemovePopupAsync();
    }
}