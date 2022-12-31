using Mafiator.Game.ViewModels;

namespace Mafiator.Game.Views;

public partial class MyRoomsView : ContentPage
{
    public MyRoomsView()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((MyRoomsViewModel)BindingContext).LoadRoomsCommand.ExecuteAsync(null);
    }

    private async void LeaveBtn_OnClicked(object sender, EventArgs e)
    {
        string roomId = ((Button)sender).CommandParameter.ToString();
        await ((MyRoomsViewModel)BindingContext).Leave(roomId);
    }
}