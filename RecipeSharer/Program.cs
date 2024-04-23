namespace RecipeSharer;
using Recipes;
using RecipeSearch;
using System;
using Users;
using Utils;
class Program
    {
        static List<User> users = new List<User>();
        static List<Recipe> recipes = new List<Recipe>();
        static List<Ingredient> ingredients = new List<Ingredient>();
        static User currentUser; 
        static RecipeOperations recipeOps = new RecipeOperations();
        static void Main(string[] args)
        {
            InitializeMockDatabase();
            Console.WriteLine("Welcome to Recipe Sharer!");

            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            currentUser = LoginOrCreateUser(username, password);
            Console.WriteLine($"Logged in as {currentUser.Username}");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Recipe");
                Console.WriteLine("2. Rate");
                Console.WriteLine("3. User");
                Console.WriteLine("4. Search");
                Console.WriteLine("5. Exit");

                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        RecipeMenu();
                        break;
                    case "2":
                        RateMenu();
                        break;
                    case "3":
                        UserMenu();
                        break;
                    case "4":
                        SearchMenu();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Exiting Recipe Sharer. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            }
        }

        static void RecipeMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nRecipe Options:");
                Console.WriteLine("1. Add Recipe");
                Console.WriteLine("2. View My Recipes");
                Console.WriteLine("3. Update Recipe");
                Console.WriteLine("4. Delete Recipe");
                Console.WriteLine("5. Back");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Recipe validrecipe = ConsoleUtils.GetValidRecipe(currentUser);
                        recipeOps.AddRecipe(currentUser,validrecipe);
                        break;
                    case "2":
                        recipeOps.ViewRecipes(currentUser);
                        break;
                    case "3":
                        Recipe validRecipe = ConsoleUtils.GetValidRecipe(currentUser);
                        recipeOps.UpdateRecipe(currentUser);
                        break;
                    case "4":
                        recipeOps.DeleteRecipe(currentUser);
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            }
        }

        static void RateMenu()
        {
            // Implement rating menu logic here
            bool back = false;
            while (!back){
                Console.WriteLine("\nRating Options:");
                Console.WriteLine("1.Rate a Recipe");
                Console.WriteLine("2.See a Recipe's Rating");
            }
        }

        static void UserMenu()
        {
            // Implement user profile menu logic here
        }

        static void SearchMenu()
        {
            // Implement search menu logic here
        }

        static User LoginOrCreateUser(string username, string password)
        {
            // Check if user exists and validate password or create new user


            return currentUser; // Simplified for demonstration
        }

        static void InitializeMockDatabase()
        {
            // Populate users and recipes lists with mock data
            users.Add(new User("ddubois1","python420"));
            users.Add(new User("j_desrosiers","csharp410"));
            users.Add(new User("pcampbell","linux440"));

            ingredients.Add(new Ingredient("Flour", 0.5, "cups", "solid"));
            ingredients.Add(new Ingredient("Sugar", 5.5, "tablespoons", "solid"));
            ingredients.Add(new Ingredient("Eggs", 3, "units", "eggs"));
            ingredients.Add(new Ingredient("Milk", 1.0/3.0, "cups", "liquid"));
            ingredients.Add(new Ingredient("Salt", 1, "teaspoons", "solid"));
            ingredients.Add(new Ingredient("Cocoa", 2, "teaspoons", "solid"));

            recipeOps.AddRecipe(users[0], new Recipe(users[0], "Scrambled Eggs", "Eggs but scrambled", new List<Ingredient>(){ingredients[2], ingredients[4]}, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2), 1));
            recipeOps.AddRecipe(users[1], new Recipe(users[1], "Cake", "Very Basic Chocolate Cake", new List<Ingredient>(){ingredients[0], ingredients[1], ingredients[2], ingredients[3], ingredients[5]}, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1), 3));
            recipeOps.AddRecipe(users[2], new Recipe(users[2], "Hot Cocoa", "Chocolate Milk but hot", new List<Ingredient>(){ingredients[1], ingredients[3], ingredients[5]}, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(40), 1));
        }
    }