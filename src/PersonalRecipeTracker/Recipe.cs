using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRecipeTrackerLibrary.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PrepTime { get; set; } = 30; // minutes
        public int CookTime { get; set; } = 30; // minutes
        public string Instructions { get; set; } = string.Empty;
        public int UserID { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Navigation properties
        public User? User { get; set; }
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient> { };
        public override string ToString()
        {
            return Name;
        }

    }
}
