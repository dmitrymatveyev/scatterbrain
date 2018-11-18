using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Scatterbrain.Controls
{
    public class JustListView : StackLayout
    {
        public DataTemplate ItemTemplate { get; set; }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(JustListView), null, propertyChanged: ItemsSourcePropertyChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var lv = (JustListView)bindable;
            lv.Children.Clear();

            void itemsSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var o in e.NewItems)
                        {
                            lv.Children.Add(ComposeView(lv, o));
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        lv.Children
                            .Where(v => e.OldItems.Contains(v.BindingContext))
                            .ToList()
                            .ForEach(v => lv.Children.Remove(v));
                        break;
                }
            }

            if (oldValue is INotifyCollectionChanged oldObservable)
            {
                oldObservable.CollectionChanged -= itemsSourceChanged;
            }

            if (newValue == null)
            {
                return;
            }
            foreach (var o in (IEnumerable)newValue)
            {
                lv.Children.Add(ComposeView(lv, o));
            }

            if (newValue is INotifyCollectionChanged newObservable)
            {
                newObservable.CollectionChanged += itemsSourceChanged;
            }
        }

        private static View ComposeView(JustListView lv, object context)
        {
            var view = (View)lv.ItemTemplate.CreateContent();
            view.BindingContext = context;
            return view;
        }
    }
}
