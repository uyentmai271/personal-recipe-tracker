using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRecipeTrackerLibrary.Models
{
    public class MealPlan
    {
        public int MealPlanID {  get; set; }
        public int UserID { get; set; }
        public int RecipeID {  get; set; }
        public DateTime MealDate {  get; set; }
        public string MealType {  get; set; } = string.Empty; // Breakfast, Lunch, Dinner
        
        // Navigation properties
        public User? User { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
