using Scatterbrain.Models;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Scatterbrain.Behaviors
{
    public class DepartmentsBehavior : SfListViewBehavior
    {
        public SfListView Departments { get; set; }

        protected override void OnAttachedTo(SfListView lv)
        {
            base.OnAttachedTo(lv);
            Departments = lv;
            Departments.Loaded += ObserveDepartmentsLoaded;
        }

        protected override void OnDetachingFrom(SfListView lv)
        {
            base.OnDetachingFrom(lv);
            Departments.Loaded -= ObserveDepartmentsLoaded;
        }

        private void ObserveDepartmentsLoaded(object sender, ListViewLoadedEventArgs e)
        {
            var itemsSorce = ((SfListView)sender).ItemsSource as ObservableCollection<Department>;
            itemsSorce.CollectionChanged += ObserveItemsSource;
        }

        private void ObserveItemsSource(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateListView(Departments);
        }
    }
}
