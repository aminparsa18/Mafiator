using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Services.GameMembers;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Hubs;
using Mafiator.Game.Models.Game;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Localization;
using Tuple = System.Tuple;

namespace Mafiator.Game.ViewModels;

public class GameViewModel : ViewModelBase
{
    //persists game hub state
    private LayoutState currentState = LayoutState.Loading;
    public LayoutState CurrentState
    {
        get => currentState;
        set => SetProperty(ref currentState, value);
    }

    //
    private bool hubConnected;
    public bool HubConnected
    {
        get => hubConnected;
        set => SetProperty(ref hubConnected, value);
    }

    //message sent on player turn
    private string message;
    public string Message
    {
        get => message;
        set => SetProperty(ref message, value);
    }

    //showing while recoding audio
    private int voiceMiliSeconds;

    private TimeSpan recordingTimer;
    public TimeSpan RecordingTimer
    {
        get => recordingTimer;
        set => SetProperty(ref recordingTimer, value);
    }

    private bool isRecording;

    //current member turn
    private PlayerDetails member;
    public PlayerDetails Member
    {
        get => member;
        set => SetProperty(ref member, value);
    }

    private Timer _timer;

    //total elapsed time of current user turn
    private int totalTime;

    //visual timer over profile picture
    private double progressTimer;
    public double ProgressTimer
    {
        get => progressTimer;
        set => SetProperty(ref progressTimer, value);
    }

    //remining time for current speaking user
    private string progressString;
    public string ProgressString
    {
        get => progressString;
        set => SetProperty(ref progressString, value);
    }

    private GameRole? role;
    public GameRole? Role
    {
        get => role;
        set => SetProperty(ref role, value);
    }

    public event EventHandler<bool> TurnChanged;
    public ObservableRangeCollection<PlayerDetails> Members { get; set; }
    public ObservableRangeCollection<GameMessageDto> Messages { get; set; }
    public IAsyncRelayCommand SendMessageCommand { get; set; }
    public IAsyncRelayCommand LikeCommand { get; set; }
    public IAsyncRelayCommand DissLikeCommand { get; set; }
    public IAsyncRelayCommand RecordAudioCommand { get; set; }
    public IAsyncRelayCommand StopRecordAudioCommand { get; set; }
    public IAsyncRelayCommand CancelRecordAudioCommand { get; set; }
    public IAsyncRelayCommand LoadDataCommand { get; set; }
    public IAsyncRelayCommand ShowRoleCommand { get; set; }
    public IAsyncRelayCommand NextCommand { get; set; }
    public IAsyncRelayCommand<string> PlayVoiceCommand { get; set; }

    private readonly IAudioRecorder _audioRecorder;
    private readonly IGameMemberApiService _gameMemberApiService;
    private readonly IMapper _mapper;
    private readonly ISubscriber<UpdateMembersEvent> _subscriber;

    private PlayerRoleResult player;
    private string gameId;

    public GameViewModel(IAudioRecorder audioRecorder, INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService,
        GameMemberApiService gameMemberApiService, IMapper mapper, ISubscriber<UpdateMembersEvent> subscriber) : base(navigationService, localizer, toastService)
    {
        _audioRecorder = audioRecorder;
        _gameMemberApiService = gameMemberApiService;
        _mapper = mapper;
        _subscriber = subscriber;
        Members = new ObservableRangeCollection<PlayerDetails>();
        Messages = new ObservableRangeCollection<GameMessageDto>();
        SendMessageCommand = new AsyncRelayCommand(SendMessage);
        LikeCommand = new AsyncRelayCommand(Like);
        DissLikeCommand = new AsyncRelayCommand(DissLike);
        RecordAudioCommand = new AsyncRelayCommand(RecordAudio);
        StopRecordAudioCommand = new AsyncRelayCommand(StopRecordAudio);
        ShowRoleCommand = new AsyncRelayCommand(ShowRole);
        NextCommand = new AsyncRelayCommand(Next);
        CancelRecordAudioCommand = new AsyncRelayCommand(CancelRecordAudio);
        LoadDataCommand = new AsyncRelayCommand(LoadData);
        PlayVoiceCommand = new AsyncRelayCommand<string>(PlayVoice);
        //CrossMediaManager.Current.StateChanged += MediaStateChanged;
        //CrossMediaManager.Current.PositionChanged += MediaPositionChanged;
        //CrossMediaManager.Current.Volume.CurrentVolume = 3;
        subscriber.Subscribe(async c => { await UpdateMembers(); });
    }

    private async Task Next() => await GameHub.Instance.InvokeAsync("Next", gameId);

    private async Task DissLike()
    {
        Messages.Add(new GameMessageDto()
        {
            Sender = true,
            Content = Message,
            Type = GameMessageType.DissLike,
            Image = Members.FirstOrDefault(m => m.Id == player.MemberId).Image,
            DisplayName = Members.FirstOrDefault(m => m.Id == player.MemberId).DisplayName,
        });

        await GameHub.Instance.InvokeAsync("SendMessage", gameId, Message, "disslike", player?.MemberId);
    }

    private async Task Like()
    {
        Messages.Add(new GameMessageDto()
        {
            Sender = true,
            Content = Message,
            Type = GameMessageType.Like,
            Image = Members.FirstOrDefault(m => m.Id == player.MemberId).Image,
            DisplayName = Members.FirstOrDefault(m => m.Id == player.MemberId).DisplayName,
        });

        await GameHub.Instance.InvokeAsync("SendMessage", gameId, Message, "like", player?.MemberId);
    }

    public override async Task InitializeAsync(object navigationData)
    {
        if (navigationData is string gameId)
        {
            this.gameId = gameId;
            await LoadDataCommand.ExecuteAsync(null);
        }
    }

    private async Task ShowRole() => await _navigationService.NavigateToPopupAsync<PlayerRoleViewModel>(Tuple.Create(Role, gameId, false));


    private async Task LoadData() => await Task.WhenAll(StartHub(), GetMembers());

    private async Task GetMembers()
    {
        await UpdateMembers();

        var result = await _gameMemberApiService.GetPlayerRole(gameId);
        if (result.IsSuccess)
        {
            Role = result.Data.Role;
            player = result.Data;
            Barrel.Current.Add("PlayerRole", result.Data, TimeSpan.FromHours(3));
            await _navigationService.NavigateToPopupAsync<PlayerRoleViewModel>(Tuple.Create(Role, gameId, true));
        }
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrEmpty(Message))
            return;
        Messages.Add(new GameMessageDto()
        {
            Sender = false,
            Content = Message,
            Type = GameMessageType.Text,
            Image = Members.FirstOrDefault(m => m.Id == player.MemberId)?.Image,
            DisplayName = Members.FirstOrDefault(m => m.Id == player.MemberId)?.DisplayName,
        });
        await GameHub.Instance.InvokeAsync("SendMessage", gameId, Message, "text", player?.MemberId);
        Message = "";
    }

    private async Task CancelRecordAudio()
    {
        isRecording = false;
        await DependencyService.Get<IAudioRecorder>().StopRecording();
    }

    private async Task StopRecordAudio()
    {
        if (!isRecording)
            return;
        isRecording = false;
        await DependencyService.Get<IAudioRecorder>().StopRecording();
        // CrossSimpleAudioPlayer.Current.Play();
        // DependencyService.Get<IAudioRecorder>().ConvertToFlac(recordPath);
        if (RecordingTimer.TotalMilliseconds < 1000)
        {
            _toastService.ShortAlert("Hold button to record", MessageType.Info);
            return;
        }

        SendVoice();
    }

    private async void SendVoice()
    {
        var id = Guid.NewGuid();
        var path = "https://mftorblob.azureedge.net/voices/" + id + ".aac";
        var message = new GameMessageDto()
        {
            Sender = true,
            Content = path,
            Type = GameMessageType.Voice,
            CurrentState = LayoutState.Loading,
            Image = Members.FirstOrDefault(m => m.Id == player.MemberId)?.Image,
            DisplayName = Members.FirstOrDefault(m => m.Id == player.MemberId)?.DisplayName,
        };
        Messages.Add(message);
        var blobServiceClient = new BlobServiceClient(
            "DefaultEndpointsProtocol=https;AccountName=mftor;AccountKey=pLoQjG6uKWpWe1vG+iVU+zKjYRpuM/tPKACmd/kM/AuBXHHfsvLOGKXsq96BusnCfrx/4St1INHVk4tibVLElA==;EndpointSuffix=core.windows.net");
        var blobContainerClient = blobServiceClient.GetBlobContainerClient("voices");
        var blobClient = blobContainerClient.GetBlobClient(id + ".aac");
        var response = await blobClient.UploadAsync(File.OpenRead(Path.Combine(Path.GetTempPath(), "myrecording.aac")), new BlobUploadOptions());
        message.CurrentState = LayoutState.Empty;

        await GameHub.Instance.InvokeAsync("SendMessage", gameId, Message, "voice", player?.MemberId);
    }

    private Task PlayVoice(string path)
    {
        //if (CrossSimpleAudioPlayer.Current.Load(path))
        //     CrossSimpleAudioPlayer.Current.Play();
        return Task.CompletedTask;
    }

    private Task RecordAudio()
    {
        isRecording = true;
        // if(CrossSimpleAudioPlayer.Current.IsPlaying)
        //   CrossSimpleAudioPlayer.Current.Pause();
        _audioRecorder.StartRecording();
        DependencyService.Get<IAudioRecorder>().StartRecording();
        //DependencyService.Get<IAudioStream>().Start();
        // recorder=new AudioRecorderService()
        //{
        //    StopRecordingOnSilence = false,
        //    StopRecordingAfterTimeout = false,
        //    TotalAudioTimeout = TimeSpan.FromSeconds(45),
        //    PreferredSampleRate = 16000
        //};
        //await recorder.StartRecording();
        Dispatcher.GetForCurrentThread().StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            if (!isRecording)
            {
                voiceMiliSeconds = 0;
                return false;
            }

            voiceMiliSeconds += 50;
            RecordingTimer = TimeSpan.FromMilliseconds(voiceMiliSeconds);
            return true;
        });
        return Task.CompletedTask;
    }

    private async void AudioInputReceived(object sender, string e)
    {
        //var str = recorder.GetAudioFileStream();
        //var sourceStream=new MemoryStream();
    }

    private async Task StartHub()
    {
        GameHub.Instance.Reconnecting += HubReconnecting;
        GameHub.Instance.Reconnected += HubReconnected;
        GameHub.Instance.Closed += HubClosed;
        if (GameHub.Instance.State == HubConnectionState.Disconnected)
            await GameHub.Instance.StartAsync();
        HubConnected = true;
        await GameHub.Instance.InvokeAsync("Subscribe", gameId);
        GameHub.Instance.On<string, string, string>("SendMessage", MessageReceived);
        GameHub.Instance.On<string>("Turn", SetTurn);
        GameHub.Instance.On<string>("AdvocacyTurn", SetAdvocacyTurn);
        GameHub.Instance.On("ShowCandidates", ShowCandidates);
        GameHub.Instance.On("ShowAdvocacyCandidates", ShowAdvocacyCandidates);
        GameHub.Instance.On<string>("GameFinish", GameFinish);
    }

    private async Task GameFinish(string arg)
    {
        await _navigationService.RemovePopupAsync();
        await _navigationService.NavigateToPopupAsync<GameFinishViewModel>(arg);
    }

    private async Task HubClosed(Exception arg)
    {
        HubConnected = false;
        _toastService.ShortAlert("Game Hub Closed", MessageType.Info);
    }

    private async Task ShowCandidates() =>
        await _navigationService.NavigateToPopupAsync<CandidatesViewModel>(Tuple.Create(Members.ToList(), gameId, false));

    private async Task ShowAdvocacyCandidates() => 
        await _navigationService.NavigateToPopupAsync<CandidatesViewModel>(Tuple.Create(Members.ToList(), gameId, true));

    private void SetTurn(string memberId)
    {
        _navigationService.RemovePopupAsync();
        Member = Members.FirstOrDefault(m => m.Id == memberId);
        totalTime = 0;
        _timer ??= new Timer(Callback, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
        var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
        TurnChanged?.Invoke(this, player?.MemberId == memberId);
    }

    private void Callback(object state)
    {
        totalTime += 100;
        ProgressTimer = 100 * (double)totalTime / 30000;
        ProgressString = TimeSpan.FromMilliseconds(30000 - totalTime).ToString(@"mm\:ss");
        if (totalTime != 30000)
            return;
        _timer?.Dispose();
        _timer = null;
    }

    private void SetAdvocacyTurn(string memberId)
    {
        _navigationService.RemovePopupAsync();
        Member = Members.FirstOrDefault(m => m.Id == memberId);
        totalTime = 0;
        _timer ??= new Timer(Callback2, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
        var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
        TurnChanged?.Invoke(this, player?.MemberId == memberId);
    }

    private void Callback2(object state)
    {
        totalTime += 100;
        ProgressTimer = 100 * (double)totalTime / 45000;
        ProgressString = TimeSpan.FromMilliseconds(45000 - totalTime).ToString(@"mm\:ss");
        if (totalTime != 45000)
            return;
        _timer?.Dispose();
        _timer = null;
    }

    private async Task HubReconnected(string arg)
    {
        HubConnected = true;
        CurrentState = LayoutState.Success;
        await GameHub.Instance.InvokeAsync("Subscribe", gameId);
    }

    private Task HubReconnecting(Exception arg)
    {
        HubConnected = false;
        CurrentState = LayoutState.Loading;
        return Task.CompletedTask;
    }

    private void MessageReceived(string msg, string type, string sender)
    {
        var message = new GameMessageDto()
        {
            Sender = false,
            Content = msg,
            Image = Members.FirstOrDefault(m => m.Id == player.MemberId)?.Image,
            DisplayName = Members.FirstOrDefault(m => m.Id == player.MemberId)?.DisplayName,
        };
        message.Type = type switch
        {
            "text" => GameMessageType.Text,
            "voice" => GameMessageType.Voice,
            "like" => GameMessageType.Like,
            "disslike" => GameMessageType.DissLike,
            _ => message.Type
        };
        if (message.Type == GameMessageType.Voice)
            message.CurrentState = LayoutState.Loading;
        Messages.Add(message);
    }

    //private void MediaStateChanged(object sender, StateChangedEventArgs e)
    //{
    //    if (e.State == MediaPlayerState.Buffering)
    //    {
    //        //if there is already a voice playing set it to default state
    //        var playings = Messages.Where(m => m.Content != SystemConstant.PlayingVoice);
    //        foreach (var playing in playings)
    //        {
    //            playing.IsPlaying = false;
    //            playing.CurrentPosition = 0;
    //            playing.CurrentState = LayoutState.Empty;
    //        }
    //    }

    //    if (e.State == MediaPlayerState.Stopped)
    //    {
    //        var playing = Messages.FirstOrDefault(m => m.Content == SystemConstant.PlayingVoice);
    //        if (playing != null)
    //        {
    //            playing.IsPlaying = false;
    //            playing.CurrentPosition = 0;
    //            playing.CurrentState = LayoutState.Empty;
    //        }

    //        var playings = Messages.Where(m => m.Content != SystemConstant.PlayingVoice);
    //        // playings.ForEach(p => p.IsPlaying = false);
    //        //  playings.ForEach(p => p.CurrentPosition = 0);
    //        //playings.ForEach(p => p.CurrentState = LayoutState.Empty);
    //    }
    //}

    //private void MediaPositionChanged(object sender, PositionChangedEventArgs e)
    //{
    //    var playing = Messages.FirstOrDefault(m => m.Content == SystemConstant.PlayingVoice);
    //    if (playing is { IsPlaying: true })
    //        playing.CurrentPosition = e.Position.TotalMilliseconds;
    //}

    public async Task StopHub() => await GameHub.Instance.StopAsync();

    public async Task UpdateMembers()
    {
        var members = await _gameMemberApiService.GetMembersOfGame(gameId);
        if (members.IsSuccess)
        {
            Members.Clear();
            var players = _mapper.Map<IEnumerable<PlayerDetails>>(members.Data);
            Members.AddRange(players);
            Barrel.Current.Add("Members", players, TimeSpan.FromHours(3));
        }
    }

    public void RecordVideo()
    {
        isRecording = true;
        Dispatcher.GetForCurrentThread().StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            if (!isRecording)
            {
                voiceMiliSeconds = 0;
                return false;
            }

            voiceMiliSeconds += 50;
            RecordingTimer = TimeSpan.FromMilliseconds(voiceMiliSeconds);
            return true;
        });
    }

    //public async Task SendVideo(string recordPath)
    //{
    //    var id = Guid.NewGuid();
    //    var path = "http://vault.mafiator.com/videos/" + id + ".mp4";
    //    using var client = new FtpClient(Constants.FtpUrl)
    //    {
    //        EncryptionMode = FtpEncryptionMode.None,
    //        ValidateAnyCertificate = true,
    //        DataConnectionEncryption = true,
    //        Credentials = new NetworkCredential("mafiator", "54Delta45!"),
    //        Port = 21,
    //        SslProtocols = SslProtocols.Tls12
    //    };
    //    await client.ConnectAsync();
    //    await using var fs2 = File.OpenRead(recordPath);
    //    var sts2 = await client.UploadAsync(fs2, "/vault.mafiator.com/videos/" + id + ".mp4", FtpRemoteExists.Resume, true);
    //    if (sts2 == FtpStatus.Success)
    //    {
    //        var message = new GameMessageDto()
    //        {
    //            Sender = true,
    //            Content = path,
    //            Type = GameMessageType.Video,
    //            CurrentState = LayoutState.Loading,
    //            Image = Members.FirstOrDefault(m => m.Id == player.MemberId)?.Image,
    //            DisplayName = Members.FirstOrDefault(m => m.Id == player.MemberId)?.DisplayName,
    //        };
    //        Messages.Add(message);
    //    }
    //    await client.DisconnectAsync();
    //    await GameHub.Instance.InvokeAsync("SendMessage", gameId, Message, "video", player?.MemberId);
    //}

    public void CancelRecordVideo()
    {
        isRecording = false;
    }
}