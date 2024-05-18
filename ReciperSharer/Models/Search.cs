using System;
using System.Collections.Generic;

namespace RecipeSearch;
using Recipes;
using Context;
using Users;
using System.Linq;


/// <summary>
/// Provides functionality to search through a collection of recipes based on various criteria.
/// </summary>
public class Search
{
    public List<string> Ingredients { get; private set; }
    public List<Tag> Tags { get; private set; }
    public string Keyword { get; private set; }
    public TimeSpan? MinDuration { get; private set; }
    public TimeSpan? MaxDuration { get; private set; }
    public double? MinimumRating { get; private set; }
    public List<Recipe> UserFavorites { get; private set; }
    public int? MaxServings { get; private set; }
    public int? MinServings { get; private set; }
    public string OwnerUsername { get; private set; }

    private readonly RecipeSharerContext _context;

    public Search(RecipeSharerContext context)
    {
        _context = context;
        Ingredients = new List<string>();
        Tags = new List<Tag>();
        UserFavorites = new List<Recipe>();
    }

    // Methods to add search criteria

   public List<Recipe> GetUserRecipes(User owner)
    {
        if (owner == null)
        {
            throw new ArgumentNullException(nameof(owner), "Owner cannot be null.");
        }

        return _context.Recipes.Where(r => r.Owner == owner).ToList();
    }

        public List<Recipe> GetFavoriteRecipes(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        return _context.Recipes.Where(recipe => user.UserFavouriteRecipes.Contains(recipe)).ToList();
    }

    /// <summary>
    /// Sets the ingredients that each recipe must contain.
    /// </summary>
    /// <param name="ingredients">A list of ingredient names.</param>
    public void SetIngredients(List<string> ingredients)
    {
        if (ingredients == null || ingredients.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Ingredients list cannot be null or contain empty strings.");
        Ingredients = ingredients;
    }
    /// <summary>
    /// Adds a tag that each recipe must include.
    /// </summary>
    /// <param name="tag">The tag to add to the search criteria.</param>
    public void AddTag(Tag tag)
    {
        if (string.IsNullOrWhiteSpace(tag.Name))
            throw new ArgumentException("Tag cannot be empty or whitespace.");
        Tags.Add(tag);
    }
    /// <summary>
    /// Sets the keyword to search for in recipe names and descriptions.
    /// </summary>
    /// <param name="keyword">The keyword for searching.</param>
    public void SetKeyword(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new ArgumentException("Keyword cannot be empty or whitespace.");
        Keyword = keyword;
    }
    /// <summary>
    /// Sets the username of the recipe owner.
    /// </summary>
    /// <param name="username">The owner's username whose recipes to search for.</param>
    public void SetOwnerUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Owner username cannot be empty or whitespace.");
        OwnerUsername = username;
    }
    /// <summary>
    /// Sets the time constraints for the recipe search.
    /// </summary>
    /// <param name="minDuration">The minimum duration.</param>
    /// <param name="maxDuration">The maximum duration.</param>
    public void SetTimeConstraints(TimeSpan? minDuration, TimeSpan? maxDuration)
    {
        if (minDuration.HasValue && minDuration.Value.TotalMinutes < 0)
            throw new ArgumentOutOfRangeException(nameof(minDuration), "Minimum duration cannot be less than zero.");
        if (maxDuration.HasValue && maxDuration.Value.TotalMinutes < 0)
            throw new ArgumentOutOfRangeException(nameof(maxDuration), "Maximum duration cannot be less than zero.");
        if (minDuration.HasValue && maxDuration.HasValue && minDuration > maxDuration)
            throw new ArgumentException("Minimum duration cannot be greater than maximum duration.");

        MinDuration = minDuration;
        MaxDuration = maxDuration;
    }

    /// <summary>
    /// Sets the servings constraints for the recipe search.
    /// </summary>
    /// <param name="minServings">The minimum number of servings.</param>
    /// <param name="maxServings">The maximum number of servings.</param>
    public void SetServingsConstraints(int? minServings, int? maxServings)
    {
        if (minServings.HasValue && minServings < 1)
            throw new ArgumentOutOfRangeException(nameof(minServings), "Minimum servings must be at least 1.");
        if (maxServings.HasValue && maxServings < 1)
            throw new ArgumentOutOfRangeException(nameof(maxServings), "Maximum servings must be at least 1.");
        if (minServings.HasValue && maxServings.HasValue && minServings > maxServings)
            throw new ArgumentException("Minimum servings cannot be greater than maximum servings.");

        MinServings = minServings;
        MaxServings = maxServings;
    }
    /// <summary>
    /// Sets the minimum rating for recipes to be included in the search results.
    /// </summary>
    /// <param name="rating">The minimum average rating.</param>
    public void SetMinimumRating(double? rating)
    {
        if (rating.HasValue && (rating < 0 || rating > 10))
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0 and 10.");
        MinimumRating = rating;
    }
    // The method to perform the actual search
    /// <summary>
    /// Searches the provided list of recipes and returns a list that matches all set criteria.
    /// </summary>
    /// <param name="allRecipes">The list of all recipes to search through.</param>
    /// <returns>A list of recipes that match the search criteria.</returns>
    public List<Recipe> PerformSearch()
    {
        // Start with all recipes
        IQueryable<Recipe> query = _context.Recipes;

        // Filter by ingredients
        if (Ingredients.Any())
        {
            query = query.Where(r => Ingredients.All(ing => r.Ingredients.Any(i => i.Name.Equals(ing, StringComparison.OrdinalIgnoreCase))));
        }

        // Filter by tags
        if (Tags.Any())
        {
            query = query.Where(r => Tags.All(tag => r.Tags.Any(t => t.Name.Equals(tag.Name, StringComparison.OrdinalIgnoreCase))));
        }

        // Filter by keyword in name or short description
        if (!string.IsNullOrEmpty(Keyword))
        {
            query = query.Where(r => r.Name.Contains(Keyword, StringComparison.OrdinalIgnoreCase) ||
                                     (r.ShortDescription != null && r.ShortDescription.Contains(Keyword, StringComparison.OrdinalIgnoreCase)));
        }

        // Filter by minimum duration
        if (MinDuration.HasValue)
        {
            query = query.Where(r => r.TotalTime >= MinDuration.Value);
        }

        // Filter by maximum duration
        if (MaxDuration.HasValue)
        {
            query = query.Where(r => r.TotalTime <= MaxDuration.Value);
        }

        // Filter by minimum rating
        if (MinimumRating.HasValue)
        {
            query = query.Where(r => r.Ratings.Any() && r.Ratings.Average(rating => rating.Score) >= MinimumRating.Value);
        }

        // Filter by user favorites
        if (UserFavorites.Any())
        {
            query = query.Where(r => UserFavorites.Contains(r));
        }

        // Filter by minimum servings
        if (MinServings.HasValue)
        {
            query = query.Where(r => r.Servings >= MinServings.Value);
        }

        // Filter by maximum servings
        if (MaxServings.HasValue)
        {
            query = query.Where(r => r.Servings <= MaxServings.Value);
        }

        // Filter by recipe owner's username
        if (!string.IsNullOrEmpty(OwnerUsername))
        {
            query = query.Where(r => r.Owner.Username.Equals(OwnerUsername, StringComparison.OrdinalIgnoreCase));
        }

        // Execute the query and return the results
        return query.ToList();
    }
}
