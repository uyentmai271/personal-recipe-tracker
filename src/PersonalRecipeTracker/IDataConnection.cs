using PersonalRecipeTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRecipeTrackerLibrary.Interfaces
{
    public interface IDataConnection
    {
        User? GetUser(int userId);

        List<Recipe> GetRecipesByUser (int userId);
        Recipe CreateRecipe(Recipe recipe);
        void UpdateRecipe (Recipe recipe);
        void DeleteRecipe(int recipeId);
        int GetTotalRecipeCount (int userId);

        List<Ingredient> GetAllIngredients();

        List<MealPlan> GetMealPlansByUser (int userId, DateTime startDate, DateTime endDate);
        void CreateMealPlan(MealPlan mealPlan);
        void DeleteMealPlan(int mealPlanId);
        int GetThisWeeksMealCount (int userId);
        List<dynamic> GetGroceryListForWeek(int userId, DateTime startDate, DateTime endDate);
    }
}
