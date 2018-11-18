using Scatterbrain.Behaviors;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scatterbrain
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TheListPage : ContentPage
    {
        public TheListPage()
        {
            InitializeComponent();
            departments.DragDropController.UpdateSource = true;
        }

        private async void AddSubject(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddSubject { BindingContext = root.BindingContext });
        }
    }
}