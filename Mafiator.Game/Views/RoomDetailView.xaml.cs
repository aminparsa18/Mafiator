using Mafiator.Game.ViewModels;

namespace Mafiator.Game.Views;

public partial class RoomDetailView : ContentPage
{
    public RoomDetailView()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.Dispatch(async () => await ((RoomDetailViewModel)BindingContext).InitializeAsync());

        //if (!Barrel.Current.Exists("RoomDetailTut"))
        //{
        //    await Task.Delay(1000);
        //    var targets = new Dictionary<View, string>()
        //    {
        //        {CopyBtn,"You can copy room code for invitations"},
        //        {ShareBtn,"Or share directly to your friends"},
        //    };
        //    var overlay = new Overlay();
        //    overlay.Show(targets, new ShowCaseConfig()
        //    {
        //        TextHorizontalPosition = HorizontalPosition.Center,
        //        TextVerticalPosition = VerticalPosition.Top
        //    });
        //    Barrel.Current.Add("RoomDetailTut", true, TimeSpan.FromDays(200));
        //}
    }
}