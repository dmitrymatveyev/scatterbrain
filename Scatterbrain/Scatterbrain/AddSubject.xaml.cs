using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scatterbrain
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddSubject : ContentPage
	{
		public AddSubject ()
		{
			InitializeComponent ();
		}

        public Keyboard Capitalized { get; } = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);

        private async void Close(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}