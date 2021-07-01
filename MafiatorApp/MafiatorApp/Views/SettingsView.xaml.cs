using System;
using MafiatorApp.Views.Templates;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : PopupPage
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            ((CheckBoxView) sender).IsChecked = !((CheckBoxView) sender).IsChecked;
        }
    }
}