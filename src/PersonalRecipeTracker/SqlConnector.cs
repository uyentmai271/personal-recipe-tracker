using Dapper;
using PersonalRecipeTrackerLibrary.Interfaces;
using PersonalRecipeTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRecipeTrackerLibrary.Data
{
    public class SqlConnector : IDataConnection
    {
        private readonly string _connectionString = GlobalConfig.ConnectionString;
        private DateTime GetWeekStart(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            var daysToSubtract = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
            var monday = date.AddDays(-daysToSubtract);
            return monday.Date;
        }
        public void CreateMealPlan(MealPlan mealPlan)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("@UserID", mealPlan.UserID);
                    p.Add("@RecipeID", mealPlan.RecipeID);
                    p.Add("@MealDate", mealPlan.MealDate);
                    p.Add("@MealType", mealPlan.MealType);

                    connection.Execute("sp_MealPlans_Insert", p, commandType: CommandType.StoredProcedure);
                }
                catch (SqlException ex) when (ex.Message.Contains("Could not find stored procedure"))
                {
                    throw new InvalidOperationException("Required stored procedure is missing. Please run the provided stored procedures script.", ex);
                }
            }
        }

        public Recipe CreateRecipe(Recipe recipe)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("@Name", recipe.Name);
                    p.Add("@Description", recipe.Description);
                    p.Add("@PrepTime", recipe.PrepTime);
                    p.Add("@CookTime", recipe.CookTime);
                    p.Add("@Instructions", recipe.Instructions);
                    p.Add("@UserID", recipe.UserID);
                    p.Add("@DateCreated", DateTime.Now);
                    p.Add("@RecipeID", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("sp_Recipes_Insert", p, commandType: CommandType.StoredProcedure);
                    recipe.RecipeID = p.Get<int>("@RecipeID");

                    foreach (var ingredient in recipe.RecipeIngredients)
                    {
                        var ingredientParams = new DynamicParameters();
                        ingredientParams.Add("@RecipeID", recipe.RecipeID);
                        ingredientParams.Add("@IngredientID", ingredient.IngredientID);
                        ingredientParams.Add("@Quantity", ingredient.Quantity);

                        connection.Execute("sp_RecipeIngredients_Insert", ingredientParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch(SqlException ex) when (ex.Message.Contains("Could not find stored procedure"))
                {
                    throw new InvalidOperationException("Required stored procedure is missing. Please run the provided stored procedures script.", ex);
                }
                return recipe;
            }
        }

        public void DeleteMealPlan(int mealPlanId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM MealPlans WHERE MealPlanID = @MealPlanId", new { MealPlanId = mealPlanId });
            }
        }

        public void DeleteRecipe(int recipeId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM RecipeIngredients WHERE RecipeID = @RecipeId", new { RecipeId = recipeId });
                connection.Execute("DELETE FROM Recipes WHERE RecipeID = @RecipeId", new { RecipeId = recipeId });
            }
        }

        public List<Ingredient> GetAllIngredients()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Ingredient>("SELECT * FROM Ingredients ORDER BY Name").ToList();
            }
        }

        public List<dynamic> GetGroceryListForWeek(int userId, DateTime startDate, DateTime endDate)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("@UserID", userId);
                    p.Add("@StartDate", startDate);
                    p.Add("@EndDate", endDate);

                    return connection.Query("sp_GetGroceryListForWeek", p, commandType: CommandType.StoredProcedure).ToList();
                }
                catch (SqlException ex) when (ex.Message.Contains("Could not find stored procedure"))
                {
                    throw new InvalidOperationException("Required stored procedure is missing. Please run the provided stored procedures script.", ex);
                }
            }
        }

        public List<MealPlan> GetMealPlansByUser(int userId, DateTime startDate, DateTime endDate)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("@UserID", userId);
                    p.Add("@StartDate", startDate);
                    p.Add("@EndDate", endDate);
                    var mealPlans = connection.Query<MealPlan>("sp_GetUserMealPlansForWeek", p, commandType: CommandType.StoredProcedure).ToList();
                    if(mealPlans != null)
                    {
                        foreach (var mealPlan in mealPlans)
                        {
                            mealPlan.Recipe = connection.QueryFirstOrDefault<Recipe>("SELECT * FROM Recipes WHERE RecipeID = @RecipeId", new { RecipeId = mealPlan.RecipeID });
                        }
                    }
                    return mealPlans ?? new List<MealPlan> { };
                }
                catch (SqlException ex) when (ex.Message.Contains("Could not find stored procedure"))
                {
                    throw new InvalidOperationException("Required stored procedure is missing. Please run the provided stored procedures script.", ex);
                }
            }
        }
        public List<Recipe> GetRecipesByUser(int userId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("@UserID", userId);

                    var recipes = connection.Query<Recipe>("sp_GetRecipesByUser", p, commandType: CommandType.StoredProcedure).ToList();
                    foreach (var recipe in recipes)
                    {
                        recipe.RecipeIngredients = connection.Query<RecipeIngredient>(
                            @"SELECT ri.*, i.Name, i.Unit
                            FROM RecipeIngredients ri
                            INNER JOIN Ingredients i ON ri.IngredientID = i.IngredientID
                            WHERE ri.RecipeID = @RecipeId",
                            new { RecipeId = recipe.RecipeID }).ToList();
                    }
                    return recipes ?? new List<Recipe>();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("Could not find stored procedure"))
                    {
                        throw new InvalidOperationException("Required stored procedure is missing. Please run the provided stored procedures script.", ex);
                    }
                    throw;
                }
            }

        }

        public int GetThisWeeksMealCount(int userId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var weekStart = GetWeekStart(DateTime.Today);
                var weekEnd = weekStart.AddDays(6);

                return connection.QuerySingle<int>(
                    "SELECT COUNT(*) FROM MealPlans WHERE UserID = @UserId AND MealDate BETWEEN @StartDate AND @EndDate",
                    new {UserId = userId, StartDate = weekStart, EndDate = weekEnd});
            }
        }

        public int GetTotalRecipeCount(int userId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<int>("SELECT COUNT(*) FROM Recipes WHERE UserID = @UserId", new { UserId = userId });
            }
        }

        public User? GetUser(int userId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE UserID = @UserId", new { UserId = userId });
            }

        }

        public void UpdateRecipe(Recipe recipe)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("@RecipeID", recipe.RecipeID);
                    p.Add("@Name", recipe.Name);
                    p.Add("@Description", recipe.Description);
                    p.Add("@PrepTime", recipe.PrepTime);
                    p.Add("@CookTime", recipe.CookTime);
                    p.Add("@Instructions", recipe.Instructions);

                    connection.Execute("sp_Recipes_Update", p, commandType: CommandType.StoredProcedure);
                }
                catch (SqlException ex) when (ex.Message.Contains("Could not find stored procedure"))
                {
                    throw new InvalidOperationException("Required stored procedure is missing. Please run the provided stored procedures script.", ex);
                }
            }
        }
    }
}
