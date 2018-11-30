using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scatterbrain
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSubject : ContentPage
    {
        public AddSubject()
        {
            InitializeComponent();

            var closed = false;
            _closeBlock = new ActionBlock<object>(o =>
            {
                if (closed)
                {
                    return;
                }
                closed = true;
                Device.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync());
            },
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
        }

        private readonly ActionBlock<object> _closeBlock;

        public static BindableProperty AddSubjCommandProperty =
            BindableProperty.Create("AddSubjCommand", typeof(ICommand), typeof(AddSubject), null);

        public ICommand AddSubjCommand
        {
            get { return (ICommand)GetValue(AddSubjCommandProperty); }
            set { SetValue(AddSubjCommandProperty, value); }
        }

        public static BindableProperty DepsProperty =
            BindableProperty.Create("Deps", typeof(IEnumerable), typeof(AddSubject), null);

        public IEnumerable Deps
        {
            get { return (IEnumerable)GetValue(DepsProperty); }
            set { SetValue(DepsProperty, value); }
        }

        public static BindableProperty DelDepCommandProperty =
            BindableProperty.Create("DelDepCommand", typeof(ICommand), typeof(AddSubject), null);

        public ICommand DelDepCommand
        {
            get { return (ICommand)GetValue(DelDepCommandProperty); }
            set { SetValue(DelDepCommandProperty, value); }
        }

        private void Close(object sender, EventArgs e)
        {
            _closeBlock.Post(null);
        }
    }
}