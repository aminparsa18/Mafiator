namespace MafiatorApp.Views.Controls;

public class TintedCachedImage : CachedImage
{
    public static BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(string), typeof(TintedCachedImage), "#00000000", propertyChanged: UpdateColor);

    public string TintColor
    {
        get => (string)GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }

    private static void UpdateColor(BindableObject bindable, object oldColor, object newColor)
    {
        var oldcolor = (string)oldColor;
        var newcolor = (string)newColor;

        if (oldcolor.Equals(newcolor) || string.IsNullOrEmpty(newcolor)) 
            return;
        var view = (TintedCachedImage)bindable;
        var transformations = new System.Collections.Generic.List<ITransformation>() {
            new TintTransformation(newcolor)
            {
                EnableSolidColor=true
            }
        };
        view.Transformations = transformations;
    }
}