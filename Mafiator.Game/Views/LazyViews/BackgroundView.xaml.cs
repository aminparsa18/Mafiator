namespace Mafiator.Game.Views.LazyViews;

public partial class BackgroundView : ContentView
{
	public BackgroundView()
	{
		InitializeComponent();
        BackgroundImage.Source = Application.Current.RequestedTheme == AppTheme.Light
                ? "day.jpg"
                : "night.jpg";
    }
}