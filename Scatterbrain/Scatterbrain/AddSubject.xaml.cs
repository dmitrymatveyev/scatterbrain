using System;
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

        public Keyboard Capitalized { get; } = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);

        public static BindableProperty AddCommandProperty =
            BindableProperty.Create("AddCommand", typeof(ICommand), typeof(AddSubject), null);

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        private void Close(object sender, EventArgs e)
        {
            _closeBlock.Post(null);
        }
    }
}