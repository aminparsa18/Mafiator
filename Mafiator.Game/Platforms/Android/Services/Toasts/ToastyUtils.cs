using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.AppCompat.Content.Res;
using AndroidX.Core.Content;
using Color = Android.Graphics.Color;
using View = Android.Views.View;

namespace Mafiator.Game.Platforms.Android.Services.Toasts
{
    public class ToastyUtils
    {
        public static Drawable TintIcon(Drawable drawable, Color tintColor)
        {
            drawable.SetColorFilter(new PorterDuffColorFilter(tintColor, PorterDuff.Mode.SrcIn));
            return drawable;
        }

        public static Drawable Tint9PatchDrawableFrame(Context context, Color tintColor)
        {
            var toastDrawable = (NinePatchDrawable)GetDrawable(context, Resource.Drawable.toast_frame);
            return TintIcon(toastDrawable, tintColor);
        }

        public static void SetBackground(View view, Drawable drawable)
        {
            view.Background = drawable;
        }

        public static Drawable GetDrawable(Context context, int id)
        {
            return AppCompatResources.GetDrawable(context, id);
        }

        public static Color GetColor(Context context, int color)
        {
            return new Color(ContextCompat.GetColor(context, color));
        }
    }
}