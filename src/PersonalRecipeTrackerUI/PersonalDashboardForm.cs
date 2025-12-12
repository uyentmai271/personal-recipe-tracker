using PersonalRecipeTrackerLibrary.Data;
using PersonalRecipeTrackerLibrary.Interfaces;
using PersonalRecipeTrackerLibrary.Models;
using System.Data.SqlClient;

namespace PersonalRecipeTrackerUI
{
    public partial class PersonalDashboardForm : Form
    {
        private readonly IDataConnection _dataConnection;
        private readonly User _currentUser;
        public PersonalDashboardForm(User currentUser)
        {
            InitializeComponent();
            _dataConnection = new SqlConnector();
            _currentUser = currentUser;

        }

        private void pnlPersonalStats_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PersonalDashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
        private void LoadDashboardData()
        {
            try
            {
                var totalRecipes = _dataConnection.GetTotalRecipeCount(_currentUser.UserID);
                lblTotalRecipes.Text = $"• Total Recipes: {totalRecipes}";

                var thisWeekMeals = _dataConnection.GetThisWeeksMealCount(_currentUser.UserID);
                lblThisWeeksMeals.Text = $"• This Week's Meals: {thisWeekMeals}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            var addRecipeForm = new PersonalAddRecipeForm(_currentUser);
            addRecipeForm.ShowDialog();
            LoadDashboardData();
        }

        private void btnViewRecipes_Click(object sender, EventArgs e)
        {
            var viewRecipesForm = new PersonalViewRecipesForm(_currentUser);
            viewRecipesForm.ShowDialog();
            LoadDashboardData();
        }

        private void btnMealPlanner_Click(object sender, EventArgs e)
        {
            try
            {
                var mealPlannerForm = new PersonalMealPlannerForm(_currentUser);
                mealPlannerForm.ShowDialog();
                LoadDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening meal planner: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGroceryList_Click(object sender, EventArgs e)
        {
            try
            {
                var groceryListForm = new PersonalGroceryListForm(_currentUser);
                groceryListForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening grocery list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
