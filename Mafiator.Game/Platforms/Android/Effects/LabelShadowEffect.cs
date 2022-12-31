using Mafiator.Game.Effects;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;

namespace MafiatorApp.Droid.Effects;

public class LabelShadowEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        try
        {
            var control = Control as Android.Widget.TextView;
            var effect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);
            if (effect == null) 
                return;
            var radius = effect.Radius;
            var distanceX = effect.DistanceX;
            var distanceY = effect.DistanceY;
            var color = effect.Color.ToAndroid();
            control.SetShadowLayer(radius, distanceX, distanceY, color);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
        }
    }

    protected override void OnDetached()
    {
    }
}