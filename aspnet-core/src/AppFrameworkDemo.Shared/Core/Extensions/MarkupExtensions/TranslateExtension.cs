using AppFrameworkDemo.Shared.Localization;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFrameworkDemo.Shared.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Text)) return Text;

            return Local.Localize(Text);
        }
    }
}