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
            NavigationPage.SetHasNavigationBar(this,false);
        }

        private async void LeaveBtn_OnClicked(object sender, EventArgs e)
        {
            var code = ((Button) sender).CommandParameter.ToString();
            await ((MyRoomsViewModel) this.BindingContext).Leave(code);
        }
    }
}