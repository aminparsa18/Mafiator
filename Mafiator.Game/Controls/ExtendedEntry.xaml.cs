using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;

namespace Mafiator.Game.Controls;

public partial class ExtendedEntry : ContentView
{
    public ExtendedEntry()
    {
        InitializeComponent();
        BindingContext = this;
        EntryControl.Text = Text;
    }

    public static event EventHandler<TextChangedEventArgs> TextChanged;

    public static readonly BindableProperty PlaceholderProperty =
       BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ExtendedEntry), "");

    public string Placeholder
    {
        get => GetValue(PlaceholderProperty).ToString();
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty TextProperty =
       BindableProperty.Create(nameof(Text), typeof(string), typeof(ExtendedEntry), "", BindingMode.OneWayToSource);

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == TextProperty.PropertyName)
        {
            EntryControl.Text = Text;
        }
    }

    private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        //((ExtendedEntry)bindable).FindByName<BorderlessEntry>(nameof(EntryControl)).Text = newValue?.ToString();
        TextChanged?.Invoke(bindable, new TextChangedEventArgs(oldValue?.ToString(), newValue?.ToString()));
    }

    public string Text
    {
        get => GetValue(TextProperty).ToString();
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty =
       BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(ExtendedEntry), false);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public static readonly BindableProperty KeyboardProperty =
      BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(ExtendedEntry), Keyboard.Default);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public static readonly BindableProperty MaxLengthProperty =
      BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(ExtendedEntry), int.MaxValue);

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ExtendedEntry), true, propertyChanged: IsValidChanged);

    private static void IsValidChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // implement when is not valid
      //  ((ExtendedEntry)bindable).FindByName<Border>(nameof(EntryBorder)).Stroke = (bool)newValue ? Colors.Green : Colors.Red;
    }

    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }

    public List<string> Errors
    {
        get => (List<string>)GetValue(ErrorsProperty);
        set => SetValue(ErrorsProperty, value);
    }

    public static readonly BindableProperty ErrorsProperty =
        BindableProperty.Create(nameof(Errors), typeof(List<string>), typeof(ExtendedEntry), new List<string>(), propertyChanged: ErrorsPropertyChanged);

    private static void ErrorsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var errors = (List<string>)newValue;
        var errorLabel = ((ExtendedEntry)bindable).FindByName<Label>(nameof(ErrorLabel));
        if (errors.Any())
        {
            errorLabel.IsVisible = true;
            errorLabel.Text = errors[0];
        }
        else
            errorLabel.IsVisible = false;
    }

    private void EntryControl_Focused(object sender, FocusEventArgs e)
    {
        PlaceHolderLabel.ScaleTo(0.8, 250, Easing.Linear);
        PlaceHolderLabel.TranslateTo(-45, -25, 250, Easing.CubicIn);
    }

    private void EntryControl_Unfocused(object sender, FocusEventArgs e)
    {
        if (!string.IsNullOrEmpty(EntryControl.Text))
            return;
        PlaceHolderLabel.ScaleTo(1, 250, Easing.Linear);
        PlaceHolderLabel.TranslateTo(0, 0, 250, Easing.CubicIn);
    }

    private void EntryControl_TextChanged(object sender, TextChangedEventArgs e)
    {
        Text = e.NewTextValue;
    }
}