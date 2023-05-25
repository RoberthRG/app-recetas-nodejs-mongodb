using System;
using System.Collections.Generic;
using System.Text;

namespace AppRecetas.Model
{
    public class Recipe
    {
        public int id { get; set; } // Agrega la propiedad "id"
        public string title { get; set; }
        public List<string> ingredients { get; set; }
        public string instructions { get; set; }
    }
}
