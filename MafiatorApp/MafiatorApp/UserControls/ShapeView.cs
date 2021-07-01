using Xamarin.Forms;

namespace MafiatorApp.UserControls
{
	public class ShapeView : BoxView
	{

		public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create<ShapeView, Color>(s => s.StrokeColor, Color.Default);

		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create<ShapeView, float>(s => s.StrokeWidth, 1f);

		public static readonly BindableProperty IndicatorPercentageProperty = BindableProperty.Create<ShapeView, float>(s => s.IndicatorPercentage, 0f);

		public static readonly BindableProperty PaddingProperty = BindableProperty.Create<ShapeView, Thickness>(s => s.Padding, default(Thickness));

	
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

		public ShapeView()
		{
		}
	}

}