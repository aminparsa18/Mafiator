using Xamarin.Forms;

namespace MafiatorApp.Helpers
{
    public class PersonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Login { get; set; }
        public DataTemplate Register { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return (int)item switch
            {
                1 => Login,
                2 => Register,
                _ => Login
            };
        }
    }
}
