using System;
using MafiatorApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewMemberView : PopupPage
    {
        public NewMemberView()
        {
            InitializeComponent();
        }

        private void ImageButton_OnClicked(object sender, EventArgs e)
        {
            var userId = ((ImageButton) sender).CommandParameter;
            ((NewMemberViewModel) BindingContext).RemoveMember(Ulid.Parse(userId.ToString()));
        }
    }
}