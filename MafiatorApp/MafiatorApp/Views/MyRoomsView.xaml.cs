using System;
using MafiatorApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyRoomsView : ContentPage
    {
        public MyRoomsView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((MyRoomsViewModel)BindingContext).LoadRoomsCommand.ExecuteAsync();
        }

        private async void LeaveBtn_OnClicked(object sender, EventArgs e)
        {
            string roomId = ((Button)sender).CommandParameter.ToString();
            await ((MyRoomsViewModel)BindingContext).Leave(roomId);
        }
    }
}