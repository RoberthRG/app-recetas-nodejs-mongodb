using AppRecetas.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Security;

namespace AppRecetas.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeDetailsPage : ContentPage
    {
        //private string Url = "http://192.168.0.106:3000/api/recipes";
        //private string Url = "http://172.20.141.14:3000/api/recipes";        
        private string Url = "http://api-recetas-nodejs-mongodb.fly.dev/api/recipes";

        private List<string> ingredientes;

        //private string recipeId;

        public RecipeDetailsPage(Model.Recipe recipe, string id)
        {
            InitializeComponent();

            txtTitulo.Text = recipe.title;
            txtInstrucciones.Text = recipe.instructions;

            txtID.Text = recipe.id.ToString();
            //recipeId = recipe.id.ToString();
            ingredientes = new List<string>();

            if (recipe.ingredients != null)
            {
                foreach (var ingrediente in recipe.ingredients)
                {
                    ingredientes.Add(ingrediente);
                }
            }

            listIngredientes.ItemsSource = ingredientes; // Asigna la lista de ingredientes al ListView
        }

        private async void ShowIngredients(object sender, EventArgs e)
        {
            await DisplayAlert("Ingredientes", "Aquí se mostrarían los ingredientes", "OK");
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirmación", "¿Estás seguro de que deseas eliminar esta receta?", "Sí", "No");

            if (answer)
            {
                using (var wc = new WebClient())
                {
                    wc.Headers.Add("Content-Type", "application/json");
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    wc.UploadString(Url + "/" + txtID.Text, "DELETE", "");
                    lblVerificar.Text = "DATOS BORRADOS CON ÉXITO";

                    await Navigation.PopAsync();
                }
            }
        }


        private async void editarReceta(object sender, EventArgs e)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/json");
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var datos = new Model.Recipe
                {
                    id = int.Parse(txtID.Text),
                    title = txtTitulo.Text,
                    instructions = txtInstrucciones.Text,
                    ingredients = new List<string>()
                };

                foreach (var item in listIngredientes.ItemsSource)
                {
                    if (item is string ingrediente)
                    {
                        datos.ingredients.Add(ingrediente);
                    }
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(datos);
                wc.UploadString(Url + "/" + txtID.Text, "PUT", json);

                // Mostrar ventana emergente de éxito
                await DisplayAlert("Éxito", "La receta se ha editado con éxito.", "Aceptar");
            }
        }


        private async void AgregarIngrediente_Clicked(object sender, EventArgs e)
        {
            string nuevoIngrediente = await DisplayPromptAsync("Nuevo Ingrediente", "Ingrese el nuevo ingrediente", "Agregar", "Cancelar", "");

            if (!string.IsNullOrWhiteSpace(nuevoIngrediente))
            {
                ingredientes.Add(nuevoIngrediente);
                listIngredientes.ItemsSource = null;
                listIngredientes.ItemsSource = ingredientes;
            }
        }
    }

}

