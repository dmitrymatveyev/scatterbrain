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
            lv.BindingContextChanged += Changed;
        }

        protected override void OnDetachingFrom(JustListView lv)
        {
            base.OnDetachingFrom(lv);
            lv.BindingContextChanged -= Changed;
        }

        private void Changed(object sender, EventArgs e)
        {
            var lv = (JustListView)sender;
            var collection = (INotifyCollectionChanged)lv.ItemsSource;
            // TODO: extract
            collection.CollectionChanged += (s, args) =>
            {
                Departments.RefreshListViewItem();
            };
        }
    }
}
