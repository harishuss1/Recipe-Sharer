namespace Recipes;
using Context;
using Microsoft.EntityFrameworkCore;
using Users;
using System;
using System.Collections.Generic;
using System.Linq;
public class RatingOperations
{
    private RecipeSharerContext _context { get; set; }

    public RatingOperations(RecipeSharerContext context)
    {
        _context = context;
    }

    private static RatingOperations? _instance;

    public static RatingOperations INSTANCE
    {
        get => _instance ??= new(RecipeSharerContext.INSTANCE!);
    }

    public void AddRating(User user, Recipe recipe, int score)
    {
        if (user == null || recipe == null)
        {
            throw new ArgumentException("User or Recipe cannot be null.");
        }

        if (score < 0 || score > 10)
        {
            throw new ArgumentOutOfRangeException("Score must be between 0 and 10.");
        }

        var userInDb = _context.Users.Find(user.UserId);
        var recipeInDb = _context.Recipes.Find(recipe.RecipeId);

        if (userInDb == null || recipeInDb == null)
        {
            throw new ArgumentException("User or Recipe not found in the database.");
        }

        Rating rating = new() { User = userInDb, Recipe = recipeInDb, Score = score };
        _context.Ratings.Add(rating);
        _context.SaveChanges();
    }

    public void RemoveRating(User user, Recipe recipe)
    {
        if (user == null || recipe == null)
        {
            throw new ArgumentException("User or Recipe cannot be null.");
        }

        var rating = _context.Ratings.FirstOrDefault(r => r.User == user && r.Recipe == recipe);
        if (rating != null)
        {
            _context.Ratings.Remove(rating);
            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("No rating from this user found.");
        }
    }
    public void UpdateRating(User user, Recipe recipe, int newScore)
    {
        if (user == null || recipe == null)
        {
            throw new ArgumentException("User or Recipe cannot be null.");
        }

        if (newScore < 0 || newScore > 10)
        {
            throw new ArgumentOutOfRangeException("Rating must be between 0 and 10.");
        }

        var rating = _context.Ratings?.FirstOrDefault(r => r.User == user && r.Recipe == recipe);
        if (rating != null)
        {
            rating.Score = newScore;
            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("No rating from this user found.");
        }
    }

    public double ViewRating(Recipe recipe)
    {
        if (recipe == null)
        {
            throw new ArgumentNullException(nameof(recipe), "Recipe can't be null");
        }

        var averageScore = _context.Ratings
            .Where(r => r.Recipe == recipe)
            .Average(r => (double?)r.Score);

        if (averageScore == null)
        {
            Console.WriteLine("No ratings available for this recipe.");
            return 0;
        }

        return averageScore.Value;
    }
}