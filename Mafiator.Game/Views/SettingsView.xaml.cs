using Mafiator.Game.ViewModels;
using Mopups.Pages;

namespace Mafiator.Game.Views;

public partial class SettingsView : PopupPage
{
    public SettingsView()
    {
        InitializeComponent();
        ((SettingsViewModel) this.BindingContext).Initial = false;
    }
}