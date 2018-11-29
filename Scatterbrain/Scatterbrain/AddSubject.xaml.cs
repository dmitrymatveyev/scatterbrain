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

        public static BindableProperty AddCommandProperty =
            BindableProperty.Create("AddCommand", typeof(ICommand), typeof(AddSubject), null);

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        public static BindableProperty DepartmentsProperty =
            BindableProperty.Create("Departments", typeof(IEnumerable), typeof(AddSubject), null);

        public IEnumerable Departments
        {
            get { return (IEnumerable)GetValue(DepartmentsProperty); }
            set { SetValue(DepartmentsProperty, value); }
        }

        public static BindableProperty DeleteDepartmentCommandProperty =
            BindableProperty.Create("DeleteDepartmentCommand", typeof(ICommand), typeof(AddSubject), null);

        public ICommand DeleteDepartmentCommand
        {
            get { return (ICommand)GetValue(DeleteDepartmentCommandProperty); }
            set { SetValue(DeleteDepartmentCommandProperty, value); }
        }

        private void Close(object sender, EventArgs e)
        {
            _closeBlock.Post(null);
        }
    }
}