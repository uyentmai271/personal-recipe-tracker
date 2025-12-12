using PersonalRecipeTrackerLibrary.Data;
using PersonalRecipeTrackerLibrary.Interfaces;
using PersonalRecipeTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalRecipeTrackerUI
{
    public partial class PersonalViewRecipesForm : Form
    {
        private readonly IDataConnection _dataConnection;
        private readonly User _currentUser;
        private List<Recipe> _recipes;
        private List<Recipe> _displayedRecipes;
        public PersonalViewRecipesForm(User currentUser)
        {
            InitializeComponent();
            _dataConnection = new SqlConnector();
            _currentUser = currentUser;
            _recipes = new List<Recipe>();
            _displayedRecipes = new List<Recipe>();
        }

        private void PersonalViewRecipesForm_Load(object sender, EventArgs e)
        {
            LoadRecipes();
        }
        private void LoadRecipes()
        {
            try
            {
                _recipes = _dataConnection.GetRecipesByUser(_currentUser.UserID);
                if (_recipes == null)
                {
                    _recipes = new List<Recipe>();
                }
                _displayedRecipes = _recipes.ToList();
                UpdateRecipeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recipes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateRecipeList()
        {
            lstRecipes.Items.Clear();
            foreach (var recipe in _displayedRecipes)
            {
                lstRecipes.Items.Add($"• {recipe.Name} (Prep: {recipe.PrepTime} min | Cook: {recipe.CookTime} min)");
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch == null)
            {
                _displayedRecipes = _recipes.ToList();
                UpdateRecipeList();
                return;
            }
            var searchTerm = txtSearch.Text?.ToLower() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _displayedRecipes = _recipes.ToList();
            }
            else
            {
                _displayedRecipes = _recipes.Where(r => r.Name.ToLower().Contains(searchTerm)).ToList();
            }
            UpdateRecipeList();
        }

        private void lstRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnDeleteRecipe_Click(object sender, EventArgs e)
        {
            if (lstRecipes.SelectedIndex >= 0 && lstRecipes.SelectedIndex < _displayedRecipes.Count)
            {
                var selectedIndex = lstRecipes.SelectedIndex;
                var recipe = _displayedRecipes[selectedIndex];

                var result = MessageBox.Show($"Are you sure you want to delete '{recipe.Name}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _dataConnection.DeleteRecipe(recipe.RecipeID);
                        LoadRecipes();
                        MessageBox.Show("Recipe deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting recipe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a recipe to delete.", "No Recipe Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnViewSelected_Click(object sender, EventArgs e)
        {
            if (lstRecipes.SelectedIndex >= 0 && lstRecipes.SelectedIndex < _displayedRecipes.Count)
            {
                var selectedIndex = lstRecipes.SelectedIndex;
                var recipe = _displayedRecipes[selectedIndex];
                DisplayRecipeDetails(recipe);
            }
            else
            {
                MessageBox.Show("Please select a recipe to view.", "No Recipe Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DisplayRecipeDetails(Recipe recipe)
        {
            var details = new StringBuilder();
            details.AppendLine($"Recipe: {recipe.Name}");
            details.AppendLine($"Description: {recipe.Description}");
            details.AppendLine($"Prep Time: {recipe.PrepTime} minutes");
            details.AppendLine($"Cook Time: {recipe.CookTime} minutes");
            details.AppendLine();
            details.AppendLine("Instructions:");
            details.AppendLine(recipe.Instructions);

            MessageBox.Show(details.ToString(), "Recipe Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
