﻿namespace RecipeSharer;
using Recipes;
using RecipeSearch;
using System;
using Users;
using Utils;
using Context;
class Program
{
    static void Main(string[] args)
    {
        RecipeSharerContext context = new RecipeSharerContext();
        UserServices uServ = new UserServices(context);
        RatingOperations ratOp = new RatingOperations(context);
        RecipeOperations recOp = new RecipeOperations(context);
        Search search = new Search(context);

        User currentUser = new User();
        Console.WriteLine("Register as new User");
        User u1 = new User();
        User u2 = new User();
        
        //Register User 1
        while (currentUser is null){
            Console.WriteLine("Enter your username: ");
            string uname = ConsoleUtils.ReadNonEmptyString("Can't be empty");
            Console.WriteLine("Enter your password: ");
            string pwd = ConsoleUtils.ReadNonEmptyString("Can't be empty");

            u1 = uServ.AddUser(uname, pwd, null, "", new List<Recipe>());
            currentUser = u1;
        }

        //Create Recipe 1
        Console.WriteLine("Enter Recipe Information");
        Recipe newRecipe = ConsoleUtils.GetValidRecipe(currentUser);

        recOp.AddRecipe(currentUser, newRecipe);
        

        //Register User 2
        currentUser = null;
        Console.WriteLine("Register as new User");
        while (currentUser is null){
            Console.WriteLine("Enter your username: ");
            string uname = ConsoleUtils.ReadNonEmptyString("Can't be empty");
            Console.WriteLine("Enter your password: ");
            string pwd = ConsoleUtils.ReadNonEmptyString("Can't be empty");

            u2 = uServ.AddUser(uname, pwd, null, "", new List<Recipe>());
            currentUser = u2;
        }

        //Search 
        Console.WriteLine("Searching by Owner");
        search.SetOwnerUsername(u1.Username);
        List<Recipe> filteredRecipes = search.PerformSearch();
        
        foreach (Recipe recipe in filteredRecipes){
            Console.WriteLine(recipe);
        }


        //Rate Recipe
        Console.WriteLine("What do you wish to rate the recipe?");
        
        int score;
        do {
            Console.WriteLine("Input rating between 0 and 10");
            score = ConsoleUtils.ReadValidInteger("invalid input");
        } while (score < 0 || score > 10);

        ratOp.AddRating(currentUser, filteredRecipes[0], score);
        Console.WriteLine($"Recipe {filteredRecipes[0].Name} rated a {score}/10");

        //Add to Favorites

        Console.WriteLine("Do you wish to add the recipe to your favorites? (y/n)");
        string fav = ConsoleUtils.ReadNonEmptyString("Needs to not be empty");

        if (fav.Equals('y')){
            uServ.AddToFavorites(filteredRecipes[0], currentUser);
            Console.WriteLine("Added to favorites");
        }
        else {
            Console.WriteLine("not added");
        }

        //Attempt to edit Recipe1
        try {
            recOp.UpdateRecipe(currentUser, filteredRecipes[0], ConsoleUtils.GetValidRecipe(currentUser));
        } catch(Exception e){
            Console.WriteLine("Can't modify recipe you don't own");
        }


        //Log Out
        currentUser = null;
        Console.WriteLine("Logged out as User 2");
        string username = "";
        string password = "";

        //Log back in as User1
        do {
            Console.WriteLine("Enter your username: ");
            username = ConsoleUtils.ReadNonEmptyString("Can't be empty");
            Console.WriteLine("Enter your password: ");
            password = ConsoleUtils.ReadNonEmptyString("Can't be empty");
        } while(!uServ.UserLogin(username, password));

        currentUser = uServ.GetUser(username);
        
        //Change user's description:
        Console.WriteLine("Enter a new description for the user");
        string newDesc = ConsoleUtils.ReadNonEmptyString("Can't be empty");
        uServ.UpdateUserProfile(currentUser, currentUser.ProfilePicture, newDesc, currentUser.UserFavouriteRecipes);

        //Get u1's recipes:
        Console.WriteLine($"{currentUser.Username}'s recipes:");
        List<Recipe> userRecipes = search.GetUserRecipes(currentUser);
        foreach (Recipe recipe in userRecipes){
            Console.WriteLine(recipe);
        }

        //Modify r1 as u1:
        try {
            recOp.UpdateRecipe(currentUser, filteredRecipes[0], ConsoleUtils.GetValidRecipe(currentUser));
        } catch(Exception e){
            Console.WriteLine("Can't modify recipe you don't own");
        }


        //Delete U1:
        uServ.DeleteUser(username, password);
        Console.WriteLine("User deleted");

        //Log back in as User2
        Console.WriteLine("Login as the 2nd user");
        do {
            Console.WriteLine("Enter your username: ");
            username = ConsoleUtils.ReadNonEmptyString("Can't be empty");
            Console.WriteLine("Enter your password: ");
            password = ConsoleUtils.ReadNonEmptyString("Can't be empty");
        } while(!uServ.UserLogin(username, password));

        currentUser = uServ.GetUser(username);

        //Delete U2
        uServ.DeleteUser(username, password);
        Console.WriteLine("User deleted");
    }


        

    // static void InitializeMockDatabase()
    // {
    //     // Populate users and recipes lists with mock data
    //     users.Add(new User("ddubois1","python420"));
    //     users.Add(new User("j_desrosiers","csharp410"));
    //     users.Add(new User("pcampbell","linux440"));

    //     ingredients.Add(new Ingredient("Flour", 0.5, "cups", "solid"));
    //     ingredients.Add(new Ingredient("Sugar", 5.5, "tablespoons", "solid"));
    //     ingredients.Add(new Ingredient("Eggs", 3, "units", "eggs"));
    //     ingredients.Add(new Ingredient("Milk", 1.0/3.0, "cups", "liquid"));
    //     ingredients.Add(new Ingredient("Salt", 1, "teaspoons", "solid"));
    //     ingredients.Add(new Ingredient("Cocoa", 2, "teaspoons", "solid"));

    //     recipeOps.AddRecipe(users[0], new Recipe(users[0], "Scrambled Eggs", "Eggs but scrambled", new List<Ingredient>(){ingredients[2], ingredients[4]}, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2), 1));
    //     recipeOps.AddRecipe(users[1], new Recipe(users[1], "Cake", "Very Basic Chocolate Cake", new List<Ingredient>(){ingredients[0], ingredients[1], ingredients[2], ingredients[3], ingredients[5]}, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1), 3));
    //     recipeOps.AddRecipe(users[2], new Recipe(users[2], "Hot Cocoa", "Chocolate Milk but hot", new List<Ingredient>(){ingredients[1], ingredients[3], ingredients[5]}, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(40), 1));
    // }
}