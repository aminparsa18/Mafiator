using System;
using System.Globalization;
using System.Reflection;
using MafiatorApp.Dtos;
using MafiatorApp.Models;
using MafiatorApp.Services;
using MafiatorApp.Services.Impl;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static IServiceProvider ServiceProvider { get; set; }

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        static ViewModelLocator()
        {
            var services = new ServiceCollection();
            services.AddMessagePipe();
            services.AddTransient<SplashScreenViewModel>();
            services.AddTransient<LanguagesViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ChatViewModel>();
            services.AddTransient<ConfirmPhoneViewModel>();
            services.AddTransient<CountriesViewModel>();
            services.AddTransient<ProfilePictureViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<MyRoomsViewModel>();
            services.AddTransient<RoomDetailViewModel>();
            services.AddTransient<JoinRoomViewModel>();
            services.AddTransient<NewRoomViewModel>();
            services.AddTransient<NewMemberViewModel>();
            services.AddTransient<NewGameViewModel>();
            services.AddTransient<SetRolesViewModel>();
            services.AddTransient<WaitingGameViewModel>();
            services.AddTransient<GameViewModel>();
            services.AddTransient<PlayerRoleViewModel>();
            services.AddTransient<CandidatesViewModel>();
            services.AddTransient<GameEventViewModel>();
            services.AddTransient<InquiryStatusViewModel>();
            services.AddTransient<WaitingViewModel>();
            services.AddTransient<StoreViewModel>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService,NavigationService>();
            services.AddSingleton<IWebApiService,WebApiService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<AvatarDto, Avatar>();
                cfg.CreateMap<GameMemberDto, PlayerDto>();
                cfg.CreateMap<PlayerDto, CandidateDto>();
            });
            ServiceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Element view))
            {
                return;
            }
          
            var viewType = view.GetType();
            var viewName = viewType.FullName?.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);
            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }

            var viewModel = ServiceProvider.GetService(viewModelType);// _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
