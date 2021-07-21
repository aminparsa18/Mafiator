using System;
using MafiatorApp.Effects.EventArgs;
using MafiatorApp.Enums;
using MafiatorApp.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameView : ContentPage
    {
        private bool keepMsgBoxDown = true;

        public GameView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((GameViewModel) BindingContext).TurnChanged += TurnChanged;
            //MessagingCenter.Subscribe<GameEventViewModel>(this, "UpdateMembers", s =>
            //{
            //    ((GameViewModel) this.BindingContext).UpdateMembers();
            //});
            //MessagingCenter.Subscribe<CandidatesViewModel>(this, "UpdateMembers", s =>
            //{
            //    ((GameViewModel)this.BindingContext).UpdateMembers();
            //});
        }


        protected override void OnDisappearing()
        {
            ((GameViewModel) BindingContext).TurnChanged -= TurnChanged;
            //MessagingCenter.Unsubscribe<GameEventViewModel>(this, "UpdateMembers");
            //MessagingCenter.Unsubscribe<CandidatesViewModel>(this, "UpdateMembers");
            base.OnDisappearing();
        }

        private void TurnChanged(object sender, bool myTurn)
        {
            if (myTurn)
            {
                MessageBox.TranslateTo(0, 0, 600, Easing.SpringOut);
                NextFrame.TranslateTo(0, 0, 600, Easing.SpringOut);
                ReactionFrame.TranslateTo(0, 200, 600, Easing.SpringIn);
                keepMsgBoxDown = false;
            }
            else if (!keepMsgBoxDown)
            {
                MessageBox.TranslateTo(0, 200, 600, Easing.SpringIn);
                NextFrame.TranslateTo(200, 0, 600, Easing.SpringIn);
                ReactionFrame.TranslateTo(0, 0, 600, Easing.SpringOut);
                keepMsgBoxDown = true;
                ((GameViewModel) BindingContext).StopRecordAudioCommand.ExecuteAsync();
                ((GameViewModel) BindingContext).SendMessageCommand.ExecuteAsync();
            }
        }


        //private async Task StartStreaming()
        //{
        //    while (recorder.IsRecording)
        //    {
        //        byte[] chunk = new byte[16];
        //        int bytesRead = await stream.ReadAsync(chunk, index, 16);
        //        if (bytesRead == 0)
        //        {
        //            await StartStreaming();
        //            break;
        //        }
        //        index += bytesRead;

        //    }
        //}


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
                    await ((GameViewModel) BindingContext).RecordAudioCommand.ExecuteAsync();
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
                await ((GameViewModel) BindingContext).StopRecordAudioCommand.ExecuteAsync();
            }
            else if (args.Type == TouchActionType.Exited)
            {
                await RecordBtn.ScaleTo(1, 300, Easing.Linear);
                CancelView.IsVisible = false;
                await ((GameViewModel) BindingContext).CancelRecordAudioCommand.ExecuteAsync();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        private async void TouchEffect2_TouchAction(object sender, TouchActionEventArgs args)
        {
            if (args.Type == TouchActionType.Pressed)
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(300));
                CancelView.IsVisible = true;
                VideoCameraView.IsVisible = true;
                await VideoCameraView.FadeTo(1, 500, Easing.Linear);
                ((GameViewModel)this.BindingContext).RecordVideo();
                await RecordVideoBtn.ScaleTo(1.5, 300, Easing.Linear);
                VideoCameraView.Shutter();

            }
            else if (args.Type == TouchActionType.Released)
            {
                await RecordVideoBtn.ScaleTo(1, 300, Easing.Linear);
                CancelView.IsVisible = false;
                VideoCameraView.IsVisible = false;
                // await ((GameViewModel) BindingContext).StopRecordAudioCommand.ExecuteAsync();
            }
            else if (args.Type == TouchActionType.Exited)
            {
                await RecordVideoBtn.ScaleTo(1, 300, Easing.Linear);
                CancelView.IsVisible = false;
                VideoCameraView.IsVisible = false;
                ((GameViewModel) BindingContext).CancelRecordVideo();
            }
        }

        private async void VideoCameraView_OnMediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            if(e.Video!=null)
              await ((GameViewModel)this.BindingContext).SendVideo(e.Video.File);
        }
    }
}