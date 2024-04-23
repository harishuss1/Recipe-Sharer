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
                    Recipe validrecipe = ConsoleUtils.GetValidRecipe(currentUser);
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
        }
    }