using Xamarin.Forms;

namespace MafiatorApp.UserControls
{
	public class ShapeView : BoxView
	{

		public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create("StrokeColor", typeof(Color), typeof(ShapeView), Color.Default);

		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create("StrokeWidth", typeof(float), typeof(ShapeView), 1f);

		public static readonly BindableProperty IndicatorPercentageProperty = BindableProperty.Create("IndicatorPercentage", typeof(float), typeof(ShapeView), 0f);

		public static readonly BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), typeof(ShapeView), default(Thickness));

	
		public Color StrokeColor
		{
			get => (Color)GetValue(StrokeColorProperty);
            set => SetValue(StrokeColorProperty, value);
        }

		public float StrokeWidth
		{
			get => (float)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }

		public float IndicatorPercentage
		{
			get => (float)GetValue(IndicatorPercentageProperty);
            set => SetValue(IndicatorPercentageProperty, value);
        }

		public Thickness Padding
		{
			get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
	}
}