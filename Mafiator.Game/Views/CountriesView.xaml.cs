using Mafiator.Game.ViewModels;
using Mopups.Pages;

namespace Mafiator.Game.Views;

public partial class CountriesView : PopupPage
{
    public CountriesView()
    {
        InitializeComponent();
    }

    //protected override async Task OnAppearingAnimationEndAsync()
    //{
    //    await base.OnAppearingAnimationEndAsync();
    //    await ((CountriesViewModel)this.BindingContext).LoadDataCommand.ExecuteAsync();
    //}

    private void InputView_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ((CountriesViewModel)this.BindingContext).Filter(e.NewTextValue);
    }
}