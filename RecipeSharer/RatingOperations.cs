using Context;
namespace Recipes;
using Users;

public class RatingOperations
{
    private readonly RecipeSharerContext _context;

    public RatingOperations(RecipeSharerContext context)
    {
        _context = context;
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

        using var context = new RecipeSharerContext();
        Rating rating = new() { User = user, Score = score };
        context.Ratings?.Add(rating);
        context.SaveChanges();
    }

    public void RemoveRating(User user, Recipe recipe)
{
    if (user == null || recipe == null)
    {
        throw new ArgumentException("User or Recipe cannot be null.");
    }

    using var context = new RecipeSharerContext();
    var rating = context.Ratings?.FirstOrDefault(r => r.User == user && r.Recipe == recipe);
    if (rating != null)
    {
        context.Ratings?.Remove(rating);
        context.SaveChanges();
    }
    else
    {
        Console.WriteLine("No rating from this user found.");
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

    using var context = new RecipeSharerContext();
    var rating = context.Ratings?.FirstOrDefault(r => r.User == user && r.Recipe == recipe);
    if (rating != null)
    {
        rating.Score = newScore;
        context.SaveChanges();
    }
    else
    {
        Console.WriteLine("No rating from this user found.");
    }
}

    public double ViewRating(Recipe recipe)
{
    if (recipe == null)
    {
        throw new ArgumentNullException(nameof(recipe), "Recipe can't be null");
    }

    using var context = new RecipeSharerContext();
    var ratings = context.Ratings.Where(r => r.Recipe == recipe).ToList();

    if (ratings.Count == 0)
    {
        Console.WriteLine("No ratings available for this recipe.");
        return 0;
    }

    double totalScore = ratings.Sum(r => r.Score);
    double averageScore = totalScore / ratings.Count;
    return averageScore;
}
}