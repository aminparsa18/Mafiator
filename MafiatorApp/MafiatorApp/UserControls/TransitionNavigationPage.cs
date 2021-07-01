using MafiatorApp.Enums;
using Xamarin.Forms;

namespace MafiatorApp.UserControls
{
    public class TransitionNavigationPage : NavigationPage
    {
        public static readonly BindableProperty TransitionTypeProperty =
             BindableProperty.Create("TransitionType", typeof(TransitionType), typeof(TransitionNavigationPage), TransitionType.SlideFromLeft);

        public TransitionType TransitionType
        {
            get => (TransitionType)GetValue(TransitionTypeProperty);
            set => SetValue(TransitionTypeProperty, value);
        }

        public TransitionNavigationPage() : base()
        {
        }

        public TransitionNavigationPage(Page root) : base(root)
        {
        }
    }
}
