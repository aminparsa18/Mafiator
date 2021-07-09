using System;
using MafiatorApp.Effects.EventArgs;
using MafiatorApp.Enums;
using MafiatorApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : ContentPage
    {
        public ChatView()
        {
            InitializeComponent();
        }
        private async void TouchEffect_TouchAction(object sender, TouchActionEventArgs args)
        {
            if (args.Type == TouchActionType.Pressed)
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
                var status2 = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status == PermissionStatus.Granted && status2 == PermissionStatus.Granted)
                {
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(300));
                    CancelView.IsVisible = true;
                    await ((GameViewModel)BindingContext).RecordAudioCommand.ExecuteAsync();
                    await RecordBtn.ScaleTo(1.5, 300, Easing.Linear);
                }

                if (status != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.Microphone>();
                if (status2 != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
            else if (args.Type == TouchActionType.Released)
            {
                await RecordBtn.ScaleTo(1, 300, Easing.Linear);
                CancelView.IsVisible = false;
                await ((GameViewModel)BindingContext).StopRecordAudioCommand.ExecuteAsync();
            }
            else if (args.Type == TouchActionType.Exited)
            {
                await RecordBtn.ScaleTo(1, 300, Easing.Linear);
                CancelView.IsVisible = false;
                await ((GameViewModel)BindingContext).CancelRecordAudioCommand.ExecuteAsync();
            }
        }

   


    }
}