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
    public partial class PersonalGroceryListForm : Form
    {
        private readonly IDataConnection _dataConnection;
        private readonly User _currentUser;
        public PersonalGroceryListForm(User currentUser)
        {
            InitializeComponent();
            _dataConnection = new SqlConnector();
            _currentUser = currentUser;

        }

        private void dtpWeekOf_ValueChanged(object sender, EventArgs e)
        {
            GenerateGroceryList();
        }

        private void PersonalGroceryListForm_Load(object sender, EventArgs e)
        {
            dtpWeekOf.Value = DateTime.Today;
            GenerateGroceryList();
        }
        private void GenerateGroceryList()
        {
            try
            {
                var weekStart = GetWeekStart(dtpWeekOf.Value);
                var weekEnd = weekStart.AddDays(6);

                var groceryItems = _dataConnection.GetGroceryListForWeek(_currentUser.UserID, weekStart, weekEnd);

                clbShoppingList.Items.Clear();

                if (groceryItems.Count == 0)
                {
                    clbShoppingList.Items.Add("No meals planned for this week.");
                    return;
                }
                foreach(var item in groceryItems)
                {
                    clbShoppingList.Items.Add($"{item.IngredientName} - {item.TotalQuantity} {item.Unit}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating grocery list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clbShoppingList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPrintList_Click(object sender, EventArgs e)
        {
            var printText = new StringBuilder();
            printText.AppendLine("PERSONAL GROCERY LIST");
            printText.AppendLine("=====================");
            printText.AppendLine();

            foreach (string item in clbShoppingList.Items)
            {
                printText.AppendLine($"• {item}");
            }
            MessageBox.Show(printText.ToString(), "Grocery List",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private DateTime GetWeekStart (DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            var daysToSubstract = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
            var monday = date.AddDays(-daysToSubstract);
            return monday.Date;
        }
    }
}
