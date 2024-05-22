namespace Recipes;
using Context;
using Microsoft.EntityFrameworkCore;
using Users;
using System;
using System.Collections.Generic;
using System.Linq;

public class RecipeOperations
{
    private RecipeSharerContext _context { get; set; }

    public RecipeOperations(RecipeSharerContext context)
    {
        _context = context;
    }
    public RecipeOperations()
    {

    }

    public void AddRecipe(User user, Recipe recipe)
    {
        if (recipe.Owner == null)
            throw new ArgumentException("Recipe must have an owner.");
        if (string.IsNullOrEmpty(recipe.Name))
            throw new ArgumentException("Recipe name cannot be null or empty.", nameof(recipe.Name));

        recipe.Owner = _context.Users.Find(user.UserId);
        _context.Recipes.Add(recipe);
        _context.SaveChanges();
    }

    public void RemoveRecipe(User user, Recipe recipe)
    {
        if (recipe.Owner == null)
            throw new ArgumentException("Recipe must have an owner.");
        if (recipe.Owner != user)
            throw new ArgumentException("Only the owner can remove the recipe.");

        var recipeToRemove = _context.Recipes.Find(recipe.RecipeId);
        if (recipeToRemove != null)
        {
            _context.Recipes.Remove(recipeToRemove);
            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Recipe not found.");
        }
    }
    public void UpdateRecipe(User user, Recipe existingRecipe, Recipe newDetails)
    {
        if (existingRecipe == null || newDetails == null)
            throw new ArgumentException("Existing recipe and new details cannot be null");

        if (existingRecipe.Owner != user)
            throw new ArgumentException("Only the owner can update the recipe.");

        if (existingRecipe.Owner != newDetails.Owner)
            throw new ArgumentException("Cannot change the owner of the recipe.");

        var recipeToUpdate = _context.Recipes.Find(existingRecipe.RecipeId);
        if (recipeToUpdate != null)
        {
            recipeToUpdate.Name = newDetails.Name;
            recipeToUpdate.ShortDescription = newDetails.ShortDescription;
            recipeToUpdate.Ingredients = newDetails.Ingredients;
            recipeToUpdate.PreparationTime = newDetails.PreparationTime;
            recipeToUpdate.CookingTime = newDetails.CookingTime;
            recipeToUpdate.Servings = newDetails.Servings;
            recipeToUpdate.Steps = new List<Step>(newDetails.Steps);
            recipeToUpdate.Tags = new List<Tag>(newDetails.Tags);

            _context.Recipes.Update(recipeToUpdate);
            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Recipe not found.");
        }
    }

    // add steps to a recipe
    public void AddStepsToRecipe(int recipeId, List<Step> steps)
    {
        if (steps == null || steps.Count == 0)
        {
            throw new ArgumentNullException(nameof(steps), "Steps cannot be null or empty.");
        }

        var recipeInDb = _context.Recipes.Find(recipeId);

        if (recipeInDb == null)
        {
            throw new ArgumentException("Recipe not found in the database.");
        }

        foreach (var step in steps)
        {
            if (string.IsNullOrWhiteSpace(step.Description))
            {
                throw new InvalidOperationException("Empty step found. Please provide a non-empty step.");
            }

            recipeInDb.Steps.Add(step);
        }

        _context.SaveChanges();
    }
    public void AddTagsToRecipe(int recipeId, List<Tag> tags)
    {
        if (tags == null || tags.Count == 0)
        {
            throw new ArgumentNullException(nameof(tags), "Tags cannot be null or empty.");
        }

        var recipeInDb = _context.Recipes.Find(recipeId);

        if (recipeInDb == null)
        {
            throw new ArgumentException("Recipe not found in the database.");
        }

        foreach (var tag in tags)
        {
            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                throw new InvalidOperationException("Empty tag found. Please provide a non-empty tag.");
            }

            recipeInDb.Tags.Add(tag);
        }

        _context.SaveChanges();
    }

    // add ingredient to recipe
    public void addIngredient(int recipeId, Ingredient ingredient)
    {
        if (ingredient == null)
        {
            throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");
        }

        var recipeInDb = _context.Recipes.Find(recipeId);

        if (recipeInDb == null)
        {
            throw new ArgumentException("Recipe not found in the database.");
        }

        recipeInDb.Ingredients.Add(ingredient);
        _context.SaveChanges();
    }

    //View all recipes
    public List<Recipe> ViewRecipes()
    {
        var recipes = _context.Recipes.ToList();
        if (recipes == null || recipes.Count == 0)
        {
            throw new ArgumentException("No Recipes Found");
        }
        return recipes;
    }

    //View user's recipe lists
    public List<Recipe> ViewUserRecipes(User owner)
    {
        if (owner == null)
        {
            throw new ArgumentNullException(nameof(owner), "Owner cannot be null.");
        }

        var userRecipes = _context.Recipes.Where(r => r.Owner == owner).ToList();

        if (userRecipes.Count == 0)
        {
            throw new ArgumentException("No Recipes Found");
        }

        return userRecipes;
    }


    public List<Recipe> ViewFavoriteRecipes(int userId)
    {
        var userInDb = _context.Users.Find(userId);

        if (userInDb == null)
        {
            throw new ArgumentException("User not found in the database.");
        }

        var favoriteRecipes = _context.Recipes
            .Where(r => userInDb.UserFavouriteRecipes.Contains(r))
            .ToList();

        if (favoriteRecipes.Count == 0)
        {
            throw new ArgumentException("No Favorite Recipes Found");
        }

        return favoriteRecipes;
    }
}
