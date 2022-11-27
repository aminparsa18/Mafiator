namespace Mafiator.Game.Controls;

public partial class PopupHeader : ContentView
{
    public PopupHeader()
    {
        InitializeComponent();
        this.TitleLabel.Text = TitleText;
        this.SubTitleLabel.Text = SubTitleText;
    }

    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create("TitleText", typeof(string), typeof(PopupHeader), string.Empty, BindingMode.OneWay, propertyChanged: TitleTextChanged);

    private static void TitleTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PopupHeader)bindable;
        control.TitleLabel.Text = newValue?.ToString();
    }

    public static readonly BindableProperty SubTitleTextProperty =
        BindableProperty.Create("SubTitleText", typeof(string), typeof(PopupHeader), string.Empty, BindingMode.OneWay, propertyChanged: SubTitleTextChanged);

    private static void SubTitleTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PopupHeader)bindable;
        control.SubTitleLabel.Text = newValue?.ToString();
    }

    public string TitleText
    {
        get => GetValue(TitleTextProperty).ToString();
        set => SetValue(TitleTextProperty, value);
    }

    public string SubTitleText
    {
        get => GetValue(SubTitleTextProperty).ToString();
        set => SetValue(SubTitleTextProperty, value);
    }
}