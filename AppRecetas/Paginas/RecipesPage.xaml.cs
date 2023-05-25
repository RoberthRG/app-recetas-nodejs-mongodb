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
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipesPage : ContentPage
    {
        public List<Model.Recipe> Recetas { get; set; }
        public bool RecetasVacias => Recetas == null || Recetas.Count == 0;
        public bool RecetasVisibles => !RecetasVacias;
        public RecipesPage()
        {
            InitializeComponent();
            BindingContext = this;

            CargarRecetas(); // Llama al método para cargar las recetas automáticamente al abrir la página
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Vuelve a cargar las recetas cada vez que la página se muestre
            CargarRecetas();
        }

        //private string Url = "http://192.168.0.106:3000/api/recipes";
        //private string Url = "http://172.20.141.14:3000/api/recipes";
        private string Url = "https://api-recetas-nodejs-mongodb.fly.dev/api/recipes";


        private async void CargarRecetas()
        {
            using (var wc = new WebClient())
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                wc.Headers.Add("Content-Type", "application/json");
                var json = await wc.DownloadStringTaskAsync(new Uri(Url));
                Recetas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.Recipe>>(json);
            }

            recipesTable.Root.Clear(); // Limpiar las filas existentes

            var headerSection = new TableSection();
            var headerCell = new ViewCell();

            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var headerLabel1 = new Label { Text = "Receta", FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            var headerLabel2 = new Label { Text = "Instrucciones", FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };

            headerGrid.Children.Add(headerLabel1, 0, 0);
            headerGrid.Children.Add(headerLabel2, 1, 0);

            headerCell.View = headerGrid;
            headerSection.Add(headerCell);
            recipesTable.Root.Add(headerSection);

            if (Recetas != null)
            {
                foreach (var receta in Recetas)
                {
                    var titleCell = new TextCell { Text = receta.title };
                    var instructionsCell = new TextCell { Text = receta.instructions };

                    var tableSection = new TableSection();
                    var viewCell = new ViewCell();

                    var grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var titleLabel = new Label { Text = receta.title, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                    var instructionsLabel = new Label { Text = receta.instructions, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };

                    grid.Children.Add(titleLabel, 0, 0);
                    grid.Children.Add(instructionsLabel, 1, 0);

                    viewCell.View = grid;
                    tableSection.Add(viewCell);
                    recipesTable.Root.Add(tableSection);

                    // Agregar el evento ItemTapped a la celda de la receta
                    viewCell.Tapped += async (s, e) =>
                    {
                        if (viewCell.View != null)
                        {
                            // Abrir una nueva página, pasando la receta seleccionada como parámetro
                            await Navigation.PushAsync(new RecipeDetailsPage(receta,null));
                        }
                    };
                }
            }
            recipesTable.IsVisible = true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Abrir una nueva página, misma que se coloca en la cima de la pila de páginas
            Navigation.PushAsync(new PruebaPostAPI());
        }
    }
}
