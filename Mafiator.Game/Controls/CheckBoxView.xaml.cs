using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace Mafiator.Game.Controls;

public partial class CheckBoxView : ContentView
{
    public CheckBoxView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked), 
        typeof(bool),
        typeof(CheckBoxView), 
        false,
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: HandleIsCheckedPropertyChanged);

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(CheckBoxView),
        "",
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: HandleTextPropertyChanged);

    private static void HandleTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var lbl = ((HorizontalStackLayout)((CheckBoxView)bindable).Children[0]).FindByName<Label>("TextLbl");
        lbl.Text = newvalue.ToString();
    }

    private static async void HandleIsCheckedPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var chkMark=((HorizontalStackLayout)((CheckBoxView) bindable).Children[0]).FindByName<Path>("CheckMark");
        if (chkMark.IsVisible)
        {
            await chkMark.FadeTo(0, 400, Easing.Linear);
            chkMark.IsVisible = !chkMark.IsVisible;
        }
        else
        {
            chkMark.IsVisible = true;
            await chkMark.FadeTo(1, 400, Easing.Linear);
        }
    }

    public bool IsChecked
    {
        get => (bool) GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private void TapGestureRecognizer_OnTapped(object sender, TappedEventArgs e) => IsChecked = !IsChecked;

}