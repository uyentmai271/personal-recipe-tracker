# Personal Recipe Tracker

A Windows Forms desktop application for managing personal recipes, planning meals, and generating grocery lists. Built with C# and SQL Server as a course project for Rapid Application Development.

## 🎯 Overview

The Personal Recipe Tracker is a comprehensive Windows Forms application that helps individuals manage their recipe collection, plan weekly meals, and automatically generate grocery lists. The application addresses common challenges in meal planning and recipe organization by providing a simple, focused solution.

### Key Capabilities

- **Recipe Management**: Create, view, search, and delete personal recipes with detailed ingredient lists
- **Meal Planning**: Plan weekly meals using an intuitive calendar interface (Breakfast, Lunch, Dinner)
- **Grocery List Generation**: Automatically generate consolidated grocery lists from meal plans
- **Dashboard Statistics**: View recipe counts and meal planning statistics
- **Ingredient Tracking**: Pre-populated ingredient database with unit measurements

## ✨ Features

### Recipe Management
- Create recipes with name, description, prep time, cook time, and instructions
- Add multiple ingredients with quantities and units
- Dynamic unit display based on selected ingredient
- Search and filter recipes by name
- View detailed recipe information including all ingredients
- Delete recipes with confirmation

### Meal Planning
- Weekly calendar view (Monday through Sunday)
- Three meal types per day (Breakfast, Lunch, Dinner)
- Visual indication of planned meals
- Week navigation (Previous/Next buttons)
- Date picker for quick week selection

### Grocery List Generation
- Automatic aggregation of ingredients from meal plans
- Quantity summation for ingredients used in multiple recipes
- Week-based grocery list generation
- Printable grocery list format

### Dashboard
- Total recipe count display
- This week's meal count
- Quick navigation to all features
- Real-time statistics updates

## 🛠 Technology Stack

- **Programming Language**: C# (.NET 8.0)
- **UI Framework**: Windows Forms (WinForms)
- **Database**: Microsoft SQL Server
- **ORM**: Dapper (Micro-ORM)
- **Database Provider**: System.Data.SqlClient
- **Development Environment**: Visual Studio 2022

## 📦 Prerequisites

Before running this application, ensure you have:

- **.NET 8.0 SDK** or later
- **Microsoft SQL Server** (2019 or later) or **SQL Server Express**
- **SQL Server Management Studio (SSMS)** or Azure Data Studio (for database setup)
- **Visual Studio 2022** (recommended) or any IDE that supports .NET 8.0

## 🗄 Database

### Database Tables

The database consists of five main tables:

- **Users**: User account information
- **Recipes**: Recipe details (name, description, prep time, cook time, instructions)
- **Ingredients**: Pre-populated ingredient list with units
- **RecipeIngredients**: Junction table linking recipes to ingredients with quantities
- **MealPlans**: Weekly meal planning information

## ⚙️ Configuration

1. **Update Connection String**

   Open `PersonalRecipeTrackerUI/App.config` and update the connection string:

   ```xml
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="Server=YOUR_SERVER_NAME;Database=PersonalRecipeTracker_DB;Integrated Security=True;TrustServerCertificate=True;" />
   </connectionStrings>
   ```

   Replace `YOUR_SERVER_NAME` with your SQL Server instance name (e.g., `localhost`, `localhost\SQLEXPRESS`, or your server name).

2. **For SQL Server Authentication** (if not using Windows Authentication):

   ```xml
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="Server=YOUR_SERVER_NAME;Database=PersonalRecipeTracker_DB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;" />
   </connectionStrings>
   ```

## 🎮 Usage

### Running the Application

1. **From Visual Studio**:
   - Open `PersonalRecipeTracker_2025.sln`
   - Set `PersonalRecipeTrackerUI` as the startup project
   - Press F5 to run

2. **From Command Line**:
   ```bash
   cd PersonalRecipeTracker_2025/PersonalRecipeTrackerUI
   dotnet run
   ```

### Application Workflow

1. **Dashboard**: The main entry point displays statistics and navigation buttons
2. **Add Recipe**: Create new recipes with ingredients and instructions
3. **View Recipes**: Browse, search, and view detailed recipe information
4. **Meal Planner**: Plan weekly meals by assigning recipes to meal slots
5. **Grocery List**: Generate consolidated grocery lists from meal plans

### Default User

The application uses a default user for demonstration. You may need to:
- Create a user in the database, or
- Modify the code to use an existing user ID

## 🗃 Database Schema

### Entity Relationships

```
Users (1) ────< (Many) Recipes
Recipes (Many) ────< RecipeIngredients >─── (Many) Ingredients
Users (1) ────< (Many) MealPlans >─── (Many) Recipes
```

### Key Relationships

- **Users → Recipes**: One-to-Many (User can have multiple recipes)
- **Recipes ↔ Ingredients**: Many-to-Many (via RecipeIngredients junction table)
- **Users → MealPlans**: One-to-Many (User can have multiple meal plans)
- **Recipes → MealPlans**: One-to-Many (Recipe can be used in multiple meal plans)

## 📜 Stored Procedures

All database operations use stored procedures for security and performance:

- `sp_Recipes_Insert` - Creates new recipes
- `sp_RecipeIngredients_Insert` - Links ingredients to recipes
- `sp_MealPlans_Insert` - Creates meal plan entries
- `sp_GetRecipesByUser` - Retrieves user's recipes
- `sp_GetUserMealPlansForWeek` - Retrieves weekly meal plans
- `sp_GetGroceryListForWeek` - Generates aggregated grocery list
- `sp_Recipes_Update` - Updates existing recipes

---

**Note**: This application was developed as a learning project and demonstrates proficiency in Windows Forms development, SQL Server database design, and rapid application development principles.
