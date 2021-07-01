using System.ComponentModel;
using Android.Content;
using MafiatorApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MafiatorApp.UserControls.ShapeView), typeof(ShapeRenderer))]
namespace MafiatorApp.Droid.Renderers
{

	public class ShapeRenderer : ViewRenderer<UserControls.ShapeView,Controls.Shape>
    {
        public ShapeRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<UserControls.ShapeView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;
            
            SetNativeControl(new Controls.Shape(Resources.DisplayMetrics.Density, Context)
            {
                ShapeView = Element
            });
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "IndicatorPercentage")
                SetNativeControl(new Controls.Shape(Resources.DisplayMetrics.Density, Context)
                {
                    ShapeView = Element
                });
        }
    }
}