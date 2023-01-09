using Mafiator.Game.ViewModels;
using Mopups.Pages;
using System.ComponentModel;

namespace Mafiator.Game.Views;

public partial class NewGameView : PopupPage
{
    public NewGameView()
    {
        InitializeComponent();
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    //MessagingCenter.Subscribe<SetRolesViewModel>(this, "SetRoles", args =>
    //    //{
    //    //    ((NewGameViewModel)BindingContext).RefreshRoles();
    //    //    SaveBtn.IsVisible = true;
    //    //});
    //}

    //protected override void OnDisappearing()
    //{
    //    MessagingCenter.Unsubscribe<SetRolesViewModel>(this, "SetRoles");
    //    base.OnDisappearing();
    //}


    private void GameTimePicker_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Time")
        {
            ((NewGameViewModel)BindingContext).SetTime(GameTimePicker.Time);
        }
    }


    private void GameDatePicker_OnUnfocused(object sender, FocusEventArgs e)
    {
        GameTimePicker.Focus();
    }

    private void DateInput_OnFocused(object sender, FocusEventArgs e)
    {
        if (e.IsFocused) 
            GameDatePicker.Focus();
    }

    private void GameTimePicker_OnUnfocused(object sender, FocusEventArgs e)
    {
      // DateInput.Unfocus();
    }
}