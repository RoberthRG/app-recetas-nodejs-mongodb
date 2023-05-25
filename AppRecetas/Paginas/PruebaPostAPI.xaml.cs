using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRecetas.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PruebaPostAPI : ContentPage
    {
        private List<string> ingredientes;

        public PruebaPostAPI()
        {
            InitializeComponent();
            ingredientes = new List<string>();
            lstIngredientes.ItemsSource = ingredientes;
        }

        //private string Url = "http://192.168.0.106:3000/api/recipes";
        //private string Url = "http://172.20.141.14:3000/api/recipes";        
        private string Url = "https://api-recetas-nodejs-mongodb.fly.dev/api/recipes";

        private void Button_AgregarIngrediente_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNuevoIngrediente.Text))
            {
                ingredientes.Add(txtNuevoIngrediente.Text);
                lstIngredientes.ItemsSource = null;
                lstIngredientes.ItemsSource = ingredientes;
                txtNuevoIngrediente.Text = string.Empty;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            using (var wc = new WebClient())
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });


                wc.Headers.Add("Content-Type", "application/json");
                var datos = new Model.Recipe
                {
                    title = txtTitulo.Text,
                    instructions = txtInstrucciones.Text,
                    ingredients = new List<string>(ingredientes) // Crear una nueva lista con los ingredientes
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(datos);
                await wc.UploadStringTaskAsync(Url, "POST", json);

                await DisplayAlert("Éxito", "La receta se ha creado con éxito.", "OK");

                //lblDatos.Text = "DATOS INSERTADOS CORRECTAMENTE";

                /*
                txtTitulo.Text = "";
                txtInstrucciones.Text = "";
                */
                await Navigation.PopAsync();
            }
        }





        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                ingredientes.Remove(e.SelectedItem.ToString());
                lstIngredientes.ItemsSource = null;
                lstIngredientes.ItemsSource = ingredientes;
            }
        }
    }
}
