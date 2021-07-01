using System;
using FFImageLoading.Svg.Forms;
using MafiatorApp.Enums;
using MafiatorApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetRolesView : PopupPage
    {
        public SetRolesView()
        {
            InitializeComponent();
        }

        private void IncBtn_OnClicked(object sender, EventArgs e)
        {

            var role =((TapGestureRecognizer) ((SvgCachedImage) sender).GestureRecognizers[0]).CommandParameter;
            ((SetRolesViewModel) BindingContext).Increment((GameRole) role);
        }

        private void DecBtn_OnClicked(object sender, EventArgs e)
        {
            var role = ((TapGestureRecognizer)((SvgCachedImage)sender).GestureRecognizers[0]).CommandParameter;
            ((SetRolesViewModel)BindingContext).Decrement((GameRole)role);
        }

    }
}