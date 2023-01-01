#if __ANDROID__
using Mafiator.Game.Platforms.Android;
using MauiTouchEffect.Platforms.Android;
using MafiatorApp.Droid.Effects;
using Mafiator.Game.Droid.Services;
#endif
using CommunityToolkit.Maui;
using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Game.Models;
using Mafiator.Game.Models.Game;
using Mafiator.Game.Services;
using Mafiator.Game.Services.Impl;
using Mafiator.Game.ViewModels;
using MauiTouchEffect;
using MessagePipe;
using Mopups.Hosting;
using Plugin.Maui.Audio;
using Plugin.MauiMTAdmob;
using Mafiator.Common.Client.Services.Users;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Mafiator.Game.Effects;

namespace Mafiator.Game;

public static class MauiProgram
{
    public static IServiceProvider Provider { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMopups()
            .UseSkiaSharp()
            .UseMauiMTAdmob()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureEffects(effects =>
            {
#if __ANDROID__
                effects.Add<TouchEffect, PlatformTouchEffect>();
                effects.Add<ShadowEffect, LabelShadowEffect>();
#endif
            });

        builder.Services.AddMessagePipe(e => e.EnableAutoRegistration = true);

        builder.Services.AddSingleton(AudioManager.Current);

        builder.Services.Scan(scan =>
            scan.FromAssemblyOf<SplashViewModel>()
            .AddClasses().AsSelf().WithTransientLifetime()
            .FromAssemblyOf<IUsersApiService>()
            .AddClasses().AsMatchingInterface().WithTransientLifetime());

        builder.Services.AddSingleton<INavigationService, NavigationService>();

#if __ANDROID__
        builder.Services.AddSingleton<IToastService, ToastService>();
        builder.Services.AddTransient<IOverlayService, OverlayService>();
#endif
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<AvatarResult, Avatar>();
            cfg.CreateMap<GameMemberResult, PlayerDetails>();
            cfg.CreateMap<PlayerDetails, CandidateDto>();
        });
        var mauiApp = builder.Build();
        Provider = mauiApp.Services;
        return mauiApp;
    }
}