using System;
using MafiatorApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RandomView : ContentPage
    {
        public RandomView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var roomId= (Guid)((Button)sender).CommandParameter;
            await ((RandomViewModel) BindingContext).OpenRoom(roomId);
        }
    }
}