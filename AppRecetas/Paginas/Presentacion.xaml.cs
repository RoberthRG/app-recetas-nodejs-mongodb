using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRecetas.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Presentacion : ContentPage
	{
		public Presentacion ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            //Abrir una nueva pagina, misma que se coloca en la cima de la pila de paginas
            Navigation.PushAsync(new RecipesPage());
        }

    }
}