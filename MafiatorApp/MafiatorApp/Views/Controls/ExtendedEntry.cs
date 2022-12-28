using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MafiatorApp.Views.Controls;

public class ExtendedEntry : Entry
{
    public ExtendedEntry()
    {
        //  Visual = VisualMarker.Material;
        Focused += OnFocused;
        Unfocused += OnUnfocused;
        ResetLineColor();
    }

    private Color _lineColorToApply;
    public Color LineColorToApply
    {
        get => _lineColorToApply;
        private set
        {
            _lineColorToApply = value;
            OnPropertyChanged(nameof(LineColorToApply));
        }
    }

    public static readonly BindableProperty LineColorProperty =
        BindableProperty.Create("LineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

    public Color LineColor
    {
        get => (Color)GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }

    public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create("IsValid", typeof(bool), typeof(ExtendedEntry), true);

    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }

    public static readonly BindableProperty InvalidLineColorProperty =
        BindableProperty.Create("InvalidLineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

    public Color InvalidLineColor
    {
        get => (Color)GetValue(InvalidLineColorProperty);
        set => SetValue(InvalidLineColorProperty, value);
    }

    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(ExtendedEntry), string.Empty);
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    public List<string> Errors
    {
        get => (List<string>)GetValue(ErrorsProperty);
        set => SetValue(ErrorsProperty, value);
    }

    public static readonly BindableProperty ErrorsProperty =
        BindableProperty.Create("Errors", typeof(List<string>), typeof(ExtendedEntry), new List<string>());
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsValidProperty.PropertyName)
            CheckValidity();
    }

    private void OnFocused(object sender, FocusEventArgs e)
    {
        IsValid = true;
        LineColorToApply = GetNormalStateLineColor();
    }

    private void OnUnfocused(object sender, FocusEventArgs e)
    {
        ResetLineColor();
    }

    private void ResetLineColor()
    {
        LineColorToApply = GetNormalStateLineColor();
    }

    private void CheckValidity()
    {
        if (!IsValid)
            LineColorToApply = InvalidLineColor;
    }

    private Color GetNormalStateLineColor()
    {
        return LineColor != Color.Default
                ? LineColor
                : TextColor;
    }
}