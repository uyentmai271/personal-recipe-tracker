using PersonalRecipeTrackerLibrary.Data;
using PersonalRecipeTrackerLibrary.Interfaces;
using PersonalRecipeTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalRecipeTrackerUI
{
    public partial class PersonalAddRecipeForm : Form
    {
        private readonly IDataConnection _dataConnection;
        private readonly User _currentUser;
        private List<Ingredient> _allIngredients;
        private List<RecipeIngredient> _selectedIngredients;
        public PersonalAddRecipeForm(User currentUser)
        {
            InitializeComponent();
            _dataConnection = new SqlConnector();
            _currentUser = currentUser;
            _allIngredients = new List<Ingredient>();
            _selectedIngredients = new List<RecipeIngredient>();
        }

        private void btnAddIngredient_Click(object sender, EventArgs e)
        {
            if (cboIngredients.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select an ingredient.", "No Ingredient Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var selectedIngredient = (Ingredient)cboIngredients.SelectedItem;
            var recipeIngredient = new RecipeIngredient
            {
                IngredientID = selectedIngredient.IngredientID,
                Quantity = (decimal)numQuantity.Value,
                Ingredient = selectedIngredient
            };
            _selectedIngredients.Add(recipeIngredient);
            UpdateIngredientsList();

            numQuantity.Value = 1;
        }
        private void UpdateIngredientsList()
        {
            lstIngredients.Items.Clear();
            foreach (var ingredient in _selectedIngredients)
            {
                lstIngredients.Items.Add($"• {ingredient.Ingredient.Name} - {ingredient.Quantity} {ingredient.Ingredient.Unit}");
            }
        }
        private void txtRecipeName_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboIngredients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboIngredients.SelectedIndex > 0 && cboIngredients.SelectedItem is Ingredient selectedIngredient)
            {
                lblIngredientUnit.Text = selectedIngredient.Unit;
            }
            else
            {
                lblIngredientUnit.Text = "-----";
            }
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtInstructions_TextChanged(object sender, EventArgs e)
        {

        }

        private void PersonalAddRecipeForm_Load(object sender, EventArgs e)
        {
            LoadIngredients();
        }
        private void LoadIngredients()
        {
            try
            {
                _allIngredients = _dataConnection.GetAllIngredients();
                cboIngredients.Items.Clear();
                cboIngredients.Items.Add("-- Select Ingredient --");

                foreach (var ingredient in _allIngredients)
                {
                    cboIngredients.Items.Add(ingredient);
                }
                cboIngredients.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ingredients: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveRecipe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRecipeName.Text))
            {
                MessageBox.Show("Please enter a recipe name.", "Recipe Name Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_selectedIngredients.Count == 0)
            {
                MessageBox.Show("Please add at least one ingredient.", "Ingredients Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var recipe = new Recipe
                {
                    Name = txtRecipeName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    PrepTime = (int)numPrepTime.Value,
                    CookTime = (int)numCookTime.Value,
                    Instructions = txtInstructions.Text.Trim(),
                    UserID = _currentUser.UserID,
                    RecipeIngredients = _selectedIngredients
                };
                _dataConnection.CreateRecipe(recipe);
                MessageBox.Show("Recipe saved successfully!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving recipe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblIngredientUnit_Click(object sender, EventArgs e)
        {

        }
    }
}
