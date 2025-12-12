using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRecipeTrackerLibrary.Models
{
    public class RecipeIngredient
    {
        public int RecipeID { get; set; }
        public int IngredientID {  get; set; }
        public decimal Quantity { get; set; } = 1;

        // Navigation properties
        public Recipe? Recipe { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
