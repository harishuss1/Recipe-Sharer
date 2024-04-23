﻿
namespace RecipeSharer;
using Recipes;
using System;
using Users;
class Program
{
    public static void Main(string[] args)
    {   
        string username;
        string password;
        do {
            username = Console.ReadLine();
        } while (username != null);
        do {
            password = Console.ReadLine();
        } while (password != null);

        User user = new User(username, password);
        Profile profile = new Profile(user);



        // Test ingredients with different quantity formats
        List<Ingredient> originalIngredients = new List<Ingredient>()
        {
            new Ingredient("Flour", 0.5, "cups", "solid"),
            new Ingredient("Sugar", 5.5, "tablespoons", "solid"),
            new Ingredient("Eggs", 3, "units", "eggs"),
            new Ingredient("Milk", 1.0/3.0, "cups", "liquid")
            // Add more ingredients as needed
        };

        Console.WriteLine("Here are the ingredients:");
        foreach (var ingredient in originalIngredients)
        {
            Console.WriteLine(ingredient);
        }

        Console.WriteLine("Would you like to scale the ingredients for more servings? Enter 'yes' or 'no'");
        string userAnswer = Console.ReadLine();
        if(userAnswer.ToLower() == "yes")
        {
            // Scale all ingredients
            int multiplier = Ingredient.GetUserMultiplier(); // Get multiplier from user
            foreach (var ingredient in originalIngredients)
            {
                ingredient.RecipeScaler(multiplier);
            } 
        }

        Console.WriteLine("Would you like to change the unit of the ingredients? Enter 'yes' or 'no'");
        string answer = Console.ReadLine();
        if(answer.ToLower() == "yes")
        {
            foreach (var ingredient in originalIngredients)
            {
                ingredient.Quantity = Ingredient.ConvertUnit(ingredient.Quantity, ingredient.UnitOfMass, ingredient.Type);
            }

            // Output converted ingredients
            Console.WriteLine("Converted Ingredients:");
            foreach (var ingredient in originalIngredients)
            {
                Console.WriteLine(ingredient);
            }
        }
        Console.ReadLine();

        Recipe recipe = new Recipe(user, "recipe", "example recipe", originalIngredients, new TimeSpan(100), new TimeSpan(100), 2);

        int rating;

        do {
            rating = Convert.ToInt32(Console.ReadLine());
        } while (rating >= 0 && rating <= 10);

        new RatingOperations().AddRating(user, recipe, rating);

        profile.addToFavorites(recipe);
    }
}