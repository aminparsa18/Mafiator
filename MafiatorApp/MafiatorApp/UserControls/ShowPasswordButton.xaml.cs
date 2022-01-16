using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowPasswordButton : ContentView
    {
        public ShowPasswordButton()
        {
            InitializeComponent();
        }
    }
}