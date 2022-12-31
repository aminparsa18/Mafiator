namespace Mafiator.Game.Controls;

public partial class PopupHeader : ContentView
{
    public PopupHeader()
    {
        InitializeComponent();
        TitleLabel.Text = TitleText;
        SubTitleLabel.Text = SubTitleText;
        PopupCloseIcon.IsVisible = ShowCloseIcon;
    }

    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText), typeof(string), typeof(PopupHeader), string.Empty, BindingMode.OneWay, propertyChanged: TitleTextChanged);

    private static void TitleTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PopupHeader)bindable;
        control.TitleLabel.Text = newValue?.ToString();
    }

    public static readonly BindableProperty SubTitleTextProperty =
        BindableProperty.Create(nameof(SubTitleText), typeof(string), typeof(PopupHeader), string.Empty, BindingMode.OneWay, propertyChanged: SubTitleTextChanged);

    private static void SubTitleTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PopupHeader)bindable;
        control.SubTitleLabel.Text = newValue?.ToString();
    }

    public static readonly BindableProperty ShowCloseIconProperty =
       BindableProperty.Create(nameof(ShowCloseIcon), typeof(bool), typeof(PopupHeader), true, BindingMode.OneWay, propertyChanged: ShowCloseIconChanged);

    private static void ShowCloseIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PopupHeader)bindable;
        control.PopupCloseIcon.IsVisible = (bool)newValue;
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

    public bool ShowCloseIcon
    {
        get => (bool)GetValue(ShowCloseIconProperty);
        set => SetValue(ShowCloseIconProperty, value);
    }
}