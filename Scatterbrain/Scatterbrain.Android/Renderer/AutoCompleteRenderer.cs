
using Scatterbrain.Controls;
using Scatterbrain.Droid.Renderer;
using Syncfusion.SfAutoComplete.XForms.Droid;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(AutoComplete), typeof(AutoCompleteRenderer))]

namespace Scatterbrain.Droid.Renderer
{
    public class AutoCompleteRenderer : SfAutoCompleteRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(Control == null)
            {
                return;
            }
            Control.GetAutoEditText().InputType = Android.Text.InputTypes.TextFlagCapCharacters | Android.Text.InputTypes.ClassText;
        }
    }
}