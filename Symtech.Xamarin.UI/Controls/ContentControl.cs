using Xamarin.Forms;

namespace Symtech.Xamarin.UI.Controls
{
    /// <summary>
    /// A view that renders its content based on a data template. Typical usage is to either set an explicit 
    /// <see cref="BindableObject.BindingContext"/> on this element or use an inhereted one, then set a display.
    /// </summary>
    public class ContentControl : ContentView
    {
        /// <summary>
        /// The content template property
        /// </summary>
        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContentControl), null, BindingMode.OneTime, propertyChanged: OnContentTemplateChanged);

        public static void OnContentTemplateChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var cp = (ContentControl)bindable;

            var template = cp.ContentTemplate;
            if (template != null)
            {
                var content = (View)template.CreateContent();
                cp.Content = content;
            }
            else
            {
                cp.Content = null;
            }
        }

        /// <summary>
        /// A <see cref="DataTemplate"/> used to render the view. This property is bindable.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
            }
        }

    }

}
