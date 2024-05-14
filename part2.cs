using System;
using System.Collections.Generic;

// The Recipe class represents a single recipe, with a name, list of ingredients, and steps
class Recipe
{
    // Name of the recipe
    public string Name { get; set; }

    // List of ingredients for the recipe
    public List<Ingredient> Ingredients { get; set; }

    // List of steps for the recipe
    public List<string> Steps { get; set; }

    // Constructor for the Recipe class
    public Recipe()
    {
        Ingredients = new List<Ingredient>();
        Steps = new List<string>();
    }

    // Method to prompt the user for ingredients and add them to the Ingredients list
    public void GetIngredientsFromUser()
    {
        Console.Write("Enter the number of ingredients: ");
        int numIngredients = int.Parse(Console.ReadLine());

        for (int i = 0; i < numIngredients; i++)
        {
            Console.Write($"Enter ingredient {i + 1} name: ");
            string name = Console.ReadLine();
            Console.Write($"Enter ingredient {i + 1} calories: ");
            int calories = int.Parse(Console.ReadLine());
            Console.Write($"Enter ingredient {i + 1} food group: ");
            string foodGroup = Console.ReadLine();
            Ingredients.Add(new Ingredient(name, calories, foodGroup));
        }
    }

    // Method to prompt the user for steps and add them to the Steps list
    public void GetStepsFromUser()
    {
        Console.Write("Enter the number of steps: ");
        int numSteps = int.Parse(Console.ReadLine());

        for (int i = 0; i < numSteps; i++)
        {
            Console.Write($"Enter step {i + 1}: ");
            string step = Console.ReadLine();
            Steps.Add(step);
        }
    }

    // Method to display the recipe to the user
    public void DisplayRecipe()
    {
        Console.WriteLine($"\nRecipe: {Name}");
        Console.WriteLine("Ingredients:");
        foreach (Ingredient ingredient in Ingredients)
        {
            Console.WriteLine($"- {ingredient.Name} ({ingredient.Calories} calories, {ingredient.FoodGroup} group)");
        }
        Console.WriteLine("\nSteps:");
        for (int i = 0; i < Steps.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + Steps[i]);
        }
    }

    // Method to calculate the total calories of the recipe
    public int CalculateTotalCalories()
    {
        int totalCalories = 0;
        foreach (Ingredient ingredient in Ingredients)
        {
            totalCalories += ingredient.Calories;
        }
        return totalCalories;
    }

    // Event to notify when the total calories exceed 300
    public event CaloriesExceededEventHandler CaloriesExceeded;

    // Method to check if the total calories exceed 300 and raise the event if so
    public void CheckCalories()
    {
        int totalCalories = CalculateTotalCalories();
        if (totalCalories > 300)
        {
            CaloriesExceeded?.Invoke(this, new CaloriesExceededEventArgs(totalCalories));
        }
    }
}

// The Ingredient class represents a single ingredient, with a name, calories, and food group
class Ingredient
{
    // Name of the ingredient
    public string Name { get; set; }

    // Calories of the ingredient
    public int Calories { get; set; }

    // Food group of the ingredient
    public string FoodGroup { get; set; }

    // Constructor for the Ingredient class
    public Ingredient(string name, int calories, string foodGroup)
    {
        Name = name;
        Calories = calories;
        FoodGroup = foodGroup;
    }
}

// Delegate for the CaloriesExceeded event
public delegate void CaloriesExceededEventHandler(Recipe recipe, CaloriesExceededEventArgs e);

// Event arguments for the CaloriesExceeded event
public class CaloriesExceededEventArgs : EventArgs
{
    // Total calories of the recipe
    public int TotalCalories { get; set; }

    // Constructor for the CaloriesExceededEventArgs class
    public CaloriesExceededEventArgs(int totalCalories)
    {
        TotalCalories = totalCalories;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Welcome the user to Recipe Manager
        Console.WriteLine("Welcome to Recipe Manager!");

        // List of recipes
        List<Recipe> recipes = new List<Recipe>();

        // Loop until the user chooses to exit
        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add a new recipe");
            Console.WriteLine("2. Display all recipes");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    // Create a new Recipe object and populate it with user input
                    Recipe recipe = new Recipe();
                    Console.Write("Enter the recipe name: ");
                    recipe.Name = Console.ReadLine();
                    recipe.GetIngredientsFromUser();
                    recipe.GetStepsFromUser();
                    recipes.Add(recipe);
                    break;
                case 2:
                    // Display all recipes to the user
                    if (recipes.Count == 0)
                    {
                        Console.WriteLine("No recipes added yet.");
                    }
                    else
                    {
                        recipes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));
                        foreach (Recipe r in recipes)
                        {
                            Console.WriteLine(r.Name);
                        }
                        Console.Write("Enter the name of the recipe to display: ");
                        string recipeName = Console.ReadLine();
                        Recipe selectedRecipe = recipes.Find(r => r.Name == recipeName);
                        if (selectedRecipe != null)
                        {
                            selectedRecipe.DisplayRecipe();
                            selectedRecipe.CheckCalories();
                        }
                        else
                        {
                            Console.WriteLine("Recipe not found.");
                        }
                    }
                    break;
                case 3:
                    // Exit the application
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
