using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Scatterbrain.Behaviors
{
    public class SfListViewBehavior : Behavior<SfListView>
    {
        public static void UpdateListView(object sender)
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
