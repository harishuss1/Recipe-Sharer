﻿namespace RecipeSharer;
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
        static User? currentUser; 
        static RecipeOperations recipeOps = new RecipeOperations();
        static RatingOperations ratingOps = new RatingOperations();
        static void Main(string[] args)
        {
            InitializeMockDatabase();
            Console.WriteLine("Welcome to Recipe Sharer!");

            while (currentUser is null){
                Console.Write("Enter your username: ");
                string username = Console.ReadLine();
                Console.Write("Enter your password: ");
                string password = Console.ReadLine();

                currentUser = LoginOrCreateUser(username, password);
            }

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
                        recipeOps.ViewUserRecipes(currentUser);

                        break;
                    case "3":
                        recipeOps.ViewUserRecipes(currentUser);
                        if (recipeOps.GetUserRecipes(currentUser).Count == 0){
                            break;
                        }
                        bool validToUpdate = false;
                        int toUpdate = -1;

                        Console.WriteLine("Select Recipe number you want to update");
                        while (validToUpdate == false){
                            toUpdate = ConsoleUtils.ReadValidInteger("not a valid choice") -1;
                            if (toUpdate >= 0 && toUpdate < recipeOps.GetUserRecipes(currentUser).Count){
                                validToUpdate = true;
                            }
                            else { Console.WriteLine("Invalid Recipe Selected"); }
                        }
                        recipeOps.UpdateRecipe(currentUser, recipeOps.GetUserRecipes(currentUser)[toUpdate], ConsoleUtils.GetValidRecipe(currentUser));
                        break;
                    case "4":
                        recipeOps.ViewUserRecipes(currentUser);
                        if (recipeOps.GetUserRecipes(currentUser).Count == 0){
                            break;
                        }
                        bool validToDelete = false;
                        int toDelete = -1;

                        Console.WriteLine("Select Recipe number you want to delete");
                        while (validToDelete == false){
                            toDelete = ConsoleUtils.ReadValidInteger("not a valid choice") -1;
                            if (toDelete >= 0 && toDelete < recipeOps.GetUserRecipes(currentUser).Count){
                                validToDelete = true;
                            }
                            else { Console.WriteLine("Invalid Recipe Selected"); }
                        }
                        recipeOps.RemoveRecipe(currentUser, recipeOps.GetUserRecipes(currentUser)[toDelete]);
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
                Console.WriteLine("3.Back");


                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        recipeOps.ViewRecipes();
                        if (recipeOps.recipes.Count == 0){
                            break;
                        }
                        bool validToRate = false;
                        int toRate = -1;

                        Console.WriteLine("Select Recipe number you want to rate");
                        while (validToRate == false){
                            toRate = ConsoleUtils.ReadValidInteger("not a valid choice") -1;
                            if (toRate >= 0 && toRate < recipeOps.recipes.Count){
                                validToRate = true;
                            }
                            else { Console.WriteLine("Invalid Recipe Selected"); }
                        }
                        bool existingRating = false;

                        //Get Rating
                        int score;
                        do {
                            Console.WriteLine("Input rating between 0 and 10");
                            score = ConsoleUtils.ReadValidInteger("invalid input");
                        } while (score < 0 || score > 10);
                        
                        //Check if User already rated
                        foreach (Rating rating in recipeOps.recipes[toRate].Ratings){
                            if (rating.User == currentUser){
                                existingRating = true;
                            }
                        }

                        //Update Rating
                        if (existingRating == true){
                            ratingOps.UpdateRating(currentUser, recipeOps.recipes[toRate], score);
                        }
                        else {
                            ratingOps.AddRating(currentUser, recipeOps.recipes[toRate], score);
                        }

                        break;
                    case "2":
                        recipeOps.ViewRecipes();
                        if (recipeOps.recipes.Count == 0){
                            break;
                        }
                        bool validToView = false;
                        int toView = -1;

                        Console.WriteLine("Select Recipe number whose rating you want to view");
                        while (validToView == false){
                            toView = ConsoleUtils.ReadValidInteger("not a valid choice") -1;
                            if (toView >= 0 && toView < recipeOps.recipes.Count){
                                validToView = true;
                            }
                            else { Console.WriteLine("Invalid Recipe Selected"); }
                        }

                        Console.WriteLine($"Average rating: {ratingOps.ViewRating(recipeOps.recipes[toView])}");
                        break;
                        
                    case "3":
                        back= true;
                        break;
                }
            }
        }

        static void UserMenu()
        {
            // Implement user profile menu logic here
            bool back = false;
            while (!back){
                Console.WriteLine("\nUser Options:");
                Console.WriteLine("1.Add a Recipe to favorites");
                Console.WriteLine("2.Remove a Recipe from favorites");
                Console.WriteLine("3.Change Password");
                Console.WriteLine("4.Back");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        recipeOps.ViewRecipes();
                        if (recipeOps.recipes.Count == 0){
                            break;
                        }
                        bool validToFavorite = false;
                        int toFavorite = -1;

                        Console.WriteLine("Select Recipe number you want to favorite");
                        while (validToFavorite == false){
                            toFavorite = ConsoleUtils.ReadValidInteger("not a valid choice") -1;
                            if (toFavorite >= 0 && toFavorite < recipeOps.recipes.Count){
                                validToFavorite = true;
                            }
                            else { Console.WriteLine("Invalid Recipe Selected"); }
                        }

                        currentUser.AddToFavorites(recipeOps.recipes[toFavorite]);

                        break;
                    case "2":
                        recipeOps.ViewFavoriteRecipes(currentUser);
                        if (recipeOps.GetFavoriteRecipes(currentUser).Count == 0){
                            break;
                        }
                        bool validToRemove = false;
                        int toRemove = -1;

                        Console.WriteLine("Select Recipe number you want to remove");
                        while (validToRemove == false){
                            toRemove = ConsoleUtils.ReadValidInteger("not a valid choice") -1;
                            if (toRemove >= 0 && toRemove < recipeOps.recipes.Count){
                                validToRemove = true;
                            }
                            else { Console.WriteLine("Invalid Recipe Selected"); }
                        }

                        currentUser.RemoveRecipeFromFavorites(recipeOps.GetFavoriteRecipes(currentUser)[toRemove]);
                        break;
                    case "3":
                        Console.WriteLine("input old password");
                        string oldPassword = ConsoleUtils.ReadNonEmptyString("needs to be not empty");
                        Console.WriteLine("input new password");
                        string newPassword = ConsoleUtils.ReadNonEmptyString("needs to be not empty");

                        try {
                            currentUser.ChangePassword(newPassword, oldPassword);
                            Console.WriteLine("Password Successfully changed.");
                        } 
                        catch (Exception e){
                            Console.WriteLine(e);
                        }

                        break;
                    case "4":
                        back = true;
                        break;
                }
            }

        }

        static void SearchMenu()
        {
            // Implement search menu logic here
            bool back = false;
            Search filters = new Search();

            while (!back){
                Console.WriteLine("\nSearch Options:");
                Console.WriteLine("1.Add filter by Ingredients");
                Console.WriteLine("2.Add filter by Tag");
                Console.WriteLine("3.Add filter by Keyword");
                Console.WriteLine("4.Add filter by Owner");
                Console.WriteLine("5.Add filter by time constraints");
                Console.WriteLine("6.Add filter by portions");
                Console.WriteLine("7.Add filter by rating");
                Console.WriteLine("8.Clear Filters");
                Console.WriteLine("9.Perform Search");
                Console.WriteLine("10.Back");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter ingredients (type 'done' to finish):");
                        string ingredientInput;
                        List<String> ingredientList = new List<string>();
                        while ((ingredientInput = Console.ReadLine()) != "done")
                        {
                            // Assuming the existence of a method to parse ingredient strings
                            ingredientList.Add((ingredientInput));
                        }
                        filters.SetIngredients(ingredientList);
                        break;

                    case "2":
                        Console.WriteLine("Enter tag");
                        string tagInput = ConsoleUtils.ReadNonEmptyString("String can't be empty");
                        filters.AddTag(tagInput);
                        break;
                    case "3":
                        Console.WriteLine("Enter Keyword");
                        string keywordInput = ConsoleUtils.ReadNonEmptyString("String can't be empty");
                        filters.SetKeyword(keywordInput);

                        break;
                    case "4":
                        Console.WriteLine("Enter Owner");
                        string ownerInput = ConsoleUtils.ReadNonEmptyString("String can't be empty");
                        filters.SetOwnerUsername(ownerInput);
                        break;
                    case "5":
                        Console.WriteLine("Enter minimum time in minutes");
                        int minInput = ConsoleUtils.ReadValidInteger("Not a valid int");
                        Console.WriteLine("Enter maximum time in minutes");
                        int maxInput = ConsoleUtils.ReadValidInteger("Not a valid int");

                        filters.SetTimeConstraints(TimeSpan.FromMinutes(minInput), TimeSpan.FromMinutes(maxInput));
                        break;
                    case "6":
                        Console.WriteLine("Enter minimum servings");
                        int minServings = ConsoleUtils.ReadValidInteger("Not a valid int");
                        Console.WriteLine("Enter maximum servings");
                        int maxServings = ConsoleUtils.ReadValidInteger("Not a valid int");

                        filters.SetServingsConstraints(minServings, maxServings);
                        break;
                    case "7":
                        Console.WriteLine("Enter minimum rating between 0 and 10");
                        int ratingInput = -1;
                        do {
                            ratingInput = ConsoleUtils.ReadValidInteger("Not a valid int");
                        } while (ratingInput < 0 && ratingInput > 10);
        
                        filters.SetMinimumRating(ratingInput);
                        break;
                    case "8":
                        filters = new Search();
                        Console.WriteLine("Filters Cleared");
                        break;
                    case "9":
                        List<Recipe> filteredRecipes = filters.PerformSearch(recipeOps.recipes);
                        int count = 0;
                        foreach (Recipe recipe in filteredRecipes){
                            count++;
                            Console.WriteLine($"{count}: {recipe.ToString()}");
                        }
                        if (count == 0) {
                            Console.WriteLine("No recipes matching filters found");
                        }

                        break;
                    case "10":
                        back = true;
                        break;
                    
                }
            }
        }

        static User LoginOrCreateUser(string username, string password)
        {
            bool userExists = false;

            // Check if user exists and validate password or create new user
            foreach (User user in users){
                if (user.Username == username){
                    userExists = true;
                    Console.WriteLine("Existing user found");
                    if (User.VerifyPassword(password, user.Salt, user.Password)){
                        Console.WriteLine("Password is valid");
                        currentUser = user;
                    }
                    else {
                        Console.WriteLine("Password is invalid");
                    }
                }
            }
            if (userExists == false){
                Console.WriteLine("User does not exist, creating new user");
                users.Add(new User(username, password));
                currentUser = users[users.Count -1];
            }
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