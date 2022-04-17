using AppFramework.Localization;
using System;
using System.Windows.Markup;

namespace AppFramework.Extensions
{
    public class TranslateExtension : MarkupExtension
    {
        public TranslateExtension()
        { }

        public TranslateExtension(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Text)) return Text;

            string val = Local.Localize(Text);
            return val;
        }
    }
}