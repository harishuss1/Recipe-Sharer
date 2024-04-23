namespace RecipeSharer;
using Recipes;
using RecipeSearch;
using System;
using Users;
using Utils;
class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Recipe Sharer!");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nPlease choose an option:");
                Console.WriteLine("1. Recipe");
                Console.WriteLine("2. Rate");
                Console.WriteLine("3. Search");
                Console.WriteLine("4. Exit");

                string mainChoice = Console.ReadLine();
                switch (mainChoice)
                {
                    case "1":
                        // Recipe operations
                        RecipeMenu();
                        break;
                    case "2":
                        // Rating operations
                        RateMenu();
                        break;
                    case "3":
                        // Search operations
                        SearchMenu();
                        break;
                    case "4":
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
                Console.WriteLine("\nRecipe Menu:");
                Console.WriteLine("1. Add Recipe");
                Console.WriteLine("2. View Recipes");
                Console.WriteLine("3. Update Recipe");
                Console.WriteLine("4. Delete Recipe");
                Console.WriteLine("5. Back to Main Menu");

                string recipeChoice = Console.ReadLine();
                switch (recipeChoice)
                {
                    case "1":
                        // Add Recipe code here
                        break;
                    case "2":
                        // View Recipes code here
                        break;
                    case "3":
                        // Update Recipe code here
                        break;
                    case "4":
                        // Delete Recipe code here
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
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nRate Menu:");
                Console.WriteLine("1. Rate a Recipe");
                Console.WriteLine("2. View Ratings");
                Console.WriteLine("3. Back to Main Menu");

                string rateChoice = Console.ReadLine();
                switch (rateChoice)
                {
                    case "1":
                        // Rate a Recipe code here
                        break;
                    case "2":
                        // View Ratings code here
                        break;
                    case "3":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            }
        }

        static void SearchMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nSearch Menu:");
                Console.WriteLine("1. Search Recipes by Name");
                Console.WriteLine("2. Search Recipes by Ingredient");
                Console.WriteLine("3. Back to Main Menu");

                string searchChoice = Console.ReadLine();
                switch (searchChoice)
                {
                    case "1":
                        // Search Recipes by Name code here
                        break;
                    case "2":
                        // Search Recipes by Ingredient code here
                        break;
                    case "3":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            }
        }
    }