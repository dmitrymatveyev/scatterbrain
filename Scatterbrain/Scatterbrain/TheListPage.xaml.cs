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
		public TheListPage ()
		{
			InitializeComponent ();
		}

        private void SfListView_Loaded(object sender, ListViewLoadedEventArgs e)
        {
            UpdateListView(sender);
            if (sender == listView)
            {
                return;
            }
            UpdateListView(listView);
        }

        private void SfListView_ChildrenChanged(object sender, ElementEventArgs e)
        {
            UpdateListView(sender);
            if (sender == listView)
            {
                return;
            }
            UpdateListView(listView);
        }

        private void UpdateListView(object sender)
        {
            if (!(sender is SfListView listView))
            {
                return;
            }

            var visualContainer = listView.GetType().GetRuntimeProperties().First(p => p.Name == "VisualContainer").GetValue(listView) as VisualContainer;

            var totalExtent = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(container => container.Name == "TotalExtent").GetValue(visualContainer);

            listView.HeightRequest = totalExtent;

            listView.RefreshView();
            listView.ForceLayout();
        }
    }
}