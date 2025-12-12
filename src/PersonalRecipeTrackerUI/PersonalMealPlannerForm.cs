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
    public partial class PersonalMealPlannerForm : Form
    {
        private readonly IDataConnection _dataConnection;
        private readonly User _currentUser;
        private List<Recipe> _recipes;
        private DateTime _currentWeekStart;
        private List<MealPlan> _mealPlans;
        public PersonalMealPlannerForm(User currentUser)
        {
            InitializeComponent();
            _dataConnection = new SqlConnector();
            _currentUser = currentUser;
            _recipes = new List<Recipe>();
            _mealPlans = new List<MealPlan>();
            _currentWeekStart = GetWeekStart(DateTime.Today);

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void lblWed_Click(object sender, EventArgs e)
        {

        }

        private void dtpWeekOf_ValueChanged(object sender, EventArgs e)
        {
            _currentWeekStart = GetWeekStart(dtpWeekOf.Value);
            LoadMealPlans();
        }

        private void btnPreviousWeek_Click(object sender, EventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            UpdateWeekDisplay();
            LoadMealPlans();
        }

        private void pnlMealPlan_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblMon_Click(object sender, EventArgs e)
        {

        }

        private void lblWeekOf_Click(object sender, EventArgs e)
        {

        }

        private void PersonalMealPlannerForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadRecipes();
                UpdateWeekDisplay();
                LoadMealPlans();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading meal planner form:{ex.Message}\n\n{ex.GetType().Name}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRecipes()
        {
            try
            {
                _recipes = _dataConnection.GetRecipesByUser(_currentUser.UserID);
                if(_recipes == null)
                {
                    _recipes = new List<Recipe>();
                }
                cboRecipes.Items.Clear();
                cboRecipes.Items.Add("-- Select Recipe --");

                foreach(var recipe in _recipes)
                {
                    cboRecipes.Items.Add(recipe);
                }
                if (cboRecipes.Items.Count > 0)
                {
                    cboRecipes.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recipes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadMealPlans()
        {
            try
            {
                var weekEnd = _currentWeekStart.AddDays(6);
                _mealPlans = _dataConnection.GetMealPlansByUser(_currentUser.UserID, _currentWeekStart, weekEnd);
                if (_mealPlans == null)
                {
                    _mealPlans= new List<MealPlan>();
                }
                UpdateMealPlanDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading meal plan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateWeekDisplay()
        {
            dtpWeekOf.Value = _currentWeekStart;
        }
        private void UpdateMealPlanDisplay()
        {
            var dayNames = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            var mealTypes = new[] { "Breakfast", "Lunch", "Dinner" };

            var buttonMap = new Dictionary<string, Button>
            {
                {"btnMonBreakfast", btnMonBreakfast},{"btnMonLunch", btnMonLunch},{"btnMonDinner", btnMonDinner},
                {"btnTueBreakfast", btnTueBreakfast},{"btnTueLunch", btnTueLunch},{"btnTueDinner", btnTueDinner},
                {"btnWedBreakfast", btnWedBreakfast},{"btnWedLunch", btnWedLunch},{"btnWedDinner", btnWedDinner},
                {"btnThuBreakfast", btnThuBreakfast},{"btnThuLunch", btnThuLunch},{"btnThuDinner", btnThuDinner},
                {"btnFriBreakfast", btnFriBreakfast},{"btnFriLunch", btnFriLunch},{"btnFriDinner", btnFriDinner},
                {"btnSatBreakfast", btnSatBreakfast},{"btnSatLunch", btnSatLunch},{"btnSatDinner", btnSatDinner},
                {"btnSunBreakfast", btnSunBreakfast},{"btnSunLunch", btnSunLunch},{"btnSunDinner", btnSunDinner}
            };
            foreach (var mealType in mealTypes)
            {
                for (int day = 0; day < 7; day++)
                {
                    var dayDate = _currentWeekStart.AddDays(day);
                    var mealPlan = _mealPlans.FirstOrDefault(mp => mp.MealDate.Date == dayDate.Date && mp.MealType == mealType);

                    var buttonName = $"btn{dayNames[day]}{mealType}";
                    if (buttonMap.TryGetValue(buttonName, out Button? button) && button != null)
                    {
                        button.Text = mealPlan?.Recipe?.Name ?? "Add Recipe";
                    }
                }
            }
        }
        private void btnMonBreakfast_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(0, "Breakfast");
        }

        private void btnMonLunch_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(0, "Lunch");
        }

        private void btnMonDinner_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(0, "Dinner");
        }

        private void btnTueBreakfast_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(1, "Breakfast");
        }

        private void btnTueLunch_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(1, "Lunch");
        }

        private void btnTueDinner_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(1, "Dinner");
        }

        private void btnWedBreakfast_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(2, "Breakfast");
        }

        private void btnWedLunch_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(2, "Lunch");
        }

        private void btnWedDinner_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(2, "Dinner");
        }

        private void btnThuBreakfast_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(3, "Breakfast");
        }

        private void btnThuLunch_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(3, "Lunch");
        }

        private void btnThuDinner_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(3, "Dinner");
        }

        private void btnFriBreakfast_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(4, "Breakfast");
        }

        private void btnFriLunch_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(4, "Lunch");
        }

        private void btnFriDinner_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(4, "Dinner");
        }

        private void btnSatBreakfast_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(5, "Breakfast");
        }

        private void btnSatLunch_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(5, "Lunch");
        }

        private void btnSatDinner_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(5, "Dinner");
        }

        private void btnSunBreakfast_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(6, "Breakfast");
        }

        private void btnSunLunch_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(6, "Lunch");
        }

        private void btnSunDinner_Click(object sender, EventArgs e)
        {
            AssignRecipeToMeal(6, "Dinner");
        }

        private void AssignRecipeToMeal(int dayIndex, string mealType)
        {
            if (cboRecipes.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a recipe first.", "No Recipe Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                if (cboRecipes.SelectedItem == null || !(cboRecipes.SelectedItem is Recipe))
                {
                    MessageBox.Show("Please select a valid recipe.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                var selectedRecipe = (Recipe)cboRecipes.SelectedItem;
                var mealDate = _currentWeekStart.AddDays(dayIndex);

                var mealPlan = new MealPlan
                {
                    UserID = _currentUser.UserID,
                    RecipeID = selectedRecipe.RecipeID,
                    MealDate = mealDate,
                    MealType = mealType,
                };

                _dataConnection.CreateMealPlan(mealPlan);
                LoadMealPlans();
                MessageBox.Show($"Added {selectedRecipe.Name} to {mealType} on {mealDate:MMM dd}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding meal plan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private DateTime GetWeekStart(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            var daysToSubstract = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
            var monday = date.AddDays(-daysToSubstract);
            return monday.Date;
        }

        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(7);
            UpdateWeekDisplay();
            LoadMealPlans();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
