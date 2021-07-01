using System;
using System.Reflection;
using System.Resources;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Resources.Texts
{
    public static class TextsTranslateManager
    {
        private const string ResourceId = "MafiatorApp.Resources.Texts.AppResources";

        public static readonly Lazy<ResourceManager> LazyResourceManager =
            new Lazy<ResourceManager>(() =>
                new ResourceManager(ResourceId, typeof(TranslateExtension)
                    .GetTypeInfo().Assembly));

        public static string Translate(string text)
        {
            var ci = Thread.CurrentThread.CurrentUICulture;
            var translation = LazyResourceManager.Value.GetString(text, ci) ?? text;
            return translation;
        }
    }

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Text == null ? "" : TextsTranslateManager.Translate(Text);
        }
    }
}
