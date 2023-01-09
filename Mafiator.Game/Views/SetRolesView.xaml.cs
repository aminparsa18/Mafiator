using Mafiator.Common.Data.Enums;
using Mafiator.Game.ViewModels;
using Mopups.Pages;

namespace Mafiator.Game.Views;

public partial class SetRolesView : PopupPage
{
    public SetRolesView()
    {
        InitializeComponent();
    }

    private void IncBtn_OnClicked(object sender, EventArgs e)
    {

        var role =((TapGestureRecognizer) ((Image) sender).GestureRecognizers[0]).CommandParameter;
        ((SetRolesViewModel) BindingContext).Increment((GameRole) role);
    }

    private void DecBtn_OnClicked(object sender, EventArgs e)
    {
        var role = ((TapGestureRecognizer)((Image)sender).GestureRecognizers[0]).CommandParameter;
        ((SetRolesViewModel)BindingContext).Decrement((GameRole)role);
    }
}