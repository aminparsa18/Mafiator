using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MafiatorApp.Helpers
{
    public class PersonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Login { get; set; }
        public DataTemplate Register { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch ((int)item)
            {
                case 1:
                    return Login;
                case 2:
                    return Register;
                default:
                    return Login;

            }

        }
    }
}
