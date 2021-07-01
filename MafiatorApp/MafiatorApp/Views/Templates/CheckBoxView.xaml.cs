using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Path = Xamarin.Forms.Shapes.Path;

namespace MafiatorApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckBoxView : ContentView
    {
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
            "IsChecked", 
            typeof(bool),
            typeof(CheckBoxView), 
            false,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: HandleIsCheckedPropertyChanged);

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Text",
            typeof(string),
            typeof(CheckBoxView),
            "",
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: HandleTextPropertyChanged);

        private static void HandleTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var lbl = ((StackLayout)((CheckBoxView)bindable).Children[0]).FindByName<Label>("TextLbl");
            lbl.Text = newvalue.ToString();
        }


        private static async void HandleIsCheckedPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var chkMark=((StackLayout)((CheckBoxView) bindable).Children[0]).FindByName<Path>("CheckMark");
            if (chkMark.IsVisible)
            {
                await chkMark.FadeTo(0, 400, Easing.Linear);
                chkMark.IsVisible = !chkMark.IsVisible;
            }
            else
            {
                chkMark.IsVisible = true;
                await chkMark.FadeTo(1, 400, Easing.Linear);
            }
        }

        public bool IsChecked
        {
            get => (bool) GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public CheckBoxView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
        }
    }
}