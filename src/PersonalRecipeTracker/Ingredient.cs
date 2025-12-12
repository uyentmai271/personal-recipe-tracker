using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRecipeTrackerLibrary.Models
{
    public class Ingredient
    {
        public int IngredientID {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = "piece";

        public override string ToString()
        {
            return Name;
        }
    }
}
