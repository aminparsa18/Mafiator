using CommunityToolkit.Maui;
using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Game.Models;
using Mafiator.Game.Models.Game;
using Mafiator.Game.Services;
using Mafiator.Game.Services.Impl;
using Mafiator.Game.ViewModels;
using Mafiator.Game.Views;
using MauiTouchEffect;
#if ANDROID
using MauiTouchEffect.Platforms.Android;
#endif
using MessagePipe;
using Mopups.Hosting;
using Plugin.Maui.Audio;
using Plugin.MauiMTAdmob;

namespace Mafiator.Game;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMopups()
            .UseMauiMTAdmob()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureEffects(effects =>
            {
#if ANDROID
                effects.Add<TouchEffect, PlatformTouchEffect>();
#endif
            });

        builder.Services.AddMessagePipe(e =>
        {
            e.EnableAutoRegistration = true;
        });

        builder.Services.AddLocalization();

        builder.Services.AddSingleton(AudioManager.Current);

        builder.Services.AddTransient<SplashView>();
        builder.Services.AddTransient<LanguagesView>();

        builder.Services.AddTransient<SplashScreenViewModel>();
        builder.Services.AddTransient<LanguagesViewModel>();
        //builder.Services.Scan(scan => 
        //    scan.FromAssemblyOf<SplashScreenViewModel>()
        //    .AddClasses().AsSelf().WithTransientLifetime()
        //    .FromAssemblyOf<SplashView>()
        //    .AddClasses().AsSelf().WithTransientLifetime()
        //    .FromAssemblyOf<IAvatarsApiService>()
        //    .AddClasses().AsImplementedInterfaces()
        //    .WithSingletonLifetime());

        builder.Services.AddSingleton<INavigationService, NavigationService>();
        //builder.Services.AddSingleton<IToastService, ToastService>();

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<AvatarResult, Avatar>();
            cfg.CreateMap<GameMemberResult, PlayerDetails>();
            cfg.CreateMap<PlayerDetails, CandidateDto>();
        });
        return builder.Build();
    }
}