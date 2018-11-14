using Scatterbrain.Models;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Scatterbrain.Behaviors
{
    public class SubjectsBehavior : Behavior<SfListView>
    {
        public SfListView Departments { get; set; }
        public SfListView Subjects { get; set; }

        protected override void OnAttachedTo(SfListView lv)
        {
            base.OnAttachedTo(lv);
            Subjects = lv;
            Subjects.Loaded += ObserveSubjectsLoaded;
        }

        protected override void OnDetachingFrom(SfListView lv)
        {
            base.OnDetachingFrom(lv);
            Subjects.Loaded -= ObserveSubjectsLoaded;
        }

        private void ObserveSubjectsLoaded(object sender, ListViewLoadedEventArgs e)
        {
            var itemsSorce = ((SfListView)sender).ItemsSource as ObservableCollection<Subject>;
            itemsSorce.CollectionChanged += ObserveItemsSource;
            ObserveItemsSource(null, null);
        }

        private void ObserveItemsSource(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateListView(Subjects);
            UpdateListView(Departments);
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
