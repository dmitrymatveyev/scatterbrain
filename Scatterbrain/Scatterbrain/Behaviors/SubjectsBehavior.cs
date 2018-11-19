using Scatterbrain.Controls;
using Scatterbrain.Models;
using Syncfusion.ListView.XForms;
using System;
using System.Collections;
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
    public class SubjectsBehavior : Behavior<JustListView>
    {
        public SfListView Departments { get; set; }

        protected override void OnAttachedTo(JustListView lv)
        {
            base.OnAttachedTo(lv);
            lv.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(JustListView lv)
        {
            base.OnDetachingFrom(lv);
            lv.BindingContextChanged -= OnBindingContextChanged;
            var collection = (INotifyCollectionChanged)lv.ItemsSource;
            collection.CollectionChanged -= OnCollectionChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            var lv = (JustListView)sender;
            var collection = (INotifyCollectionChanged)lv.ItemsSource;
            collection.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Departments.RefreshListViewItem();
        }
    }
}
