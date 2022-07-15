
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ESATouristGuide.Helpers
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        public string Text { get; set; }
        object IMarkupExtension.ProvideValue( IServiceProvider serviceProvider )
        {
            return ProvideValue(serviceProvider);
        }

        public BindingBase ProvideValue( IServiceProvider serviceProvider )
        {
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay ,
                Path = $"[{Text}]" ,
                Source = LocalizationResourceManager.Instance ,
            };
            return binding;
        }
    }
}
