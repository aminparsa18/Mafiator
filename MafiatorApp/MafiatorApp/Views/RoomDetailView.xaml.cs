using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Common.Client.Cache;
using MafiatorApp.Models;
using MafiatorApp.Services;
using MafiatorApp.UserControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomDetailView : ContentPage
    {
        public RoomDetailView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!Barrel.Current.Exists("RoomDetailTut"))
            {
                await Task.Delay(1000);
                var targets = new Dictionary<View, string>()
                {
                    {CopyBtn,"You can copy room code for invitations"},
                    {ShareBtn,"Or share directly to your friends"},
                };
                var overlay = new Overlay();
                overlay.Show(targets, new ShowCaseConfig()
                {
                    TextHorizontalPosition = HorizontalPosition.Center,
                    TextVerticalPosition = VerticalPosition.Top
                });
                Barrel.Current.Add("RoomDetailTut", true, TimeSpan.FromDays(200));
            }
        }



        private async void CopyBtn_OnClicked(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(RoomCodeSpan.Text);
            DependencyService.Get<IAlert>().ShortAlert("Code copied to clipboard",MessageType.Success);
        }
    }
}