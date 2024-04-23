namespace Recipes;
using Users;

public class RatingOperations
{
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

    Rating rating = new Rating(user, score);
    recipe.Ratings.Add(rating);
    }

    public void RemoveRating(User user, Recipe recipe)
    {
        if (user == null || recipe == null)
        {
            Console.WriteLine("User or Recipe cannot be null.");
            return;
        }

        var rating = recipe.Ratings.FirstOrDefault(r => r.User == user);
        if (rating != null)
        {
            recipe.Ratings.Remove(rating);
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
            Console.WriteLine("User or Recipe cannot be null.");
            return;
        }

        if (newScore < 0 || newScore > 10)
        {
            Console.WriteLine("Rating must be between 0 and 10.");
            return;
        }

        var rating = recipe.Ratings.FirstOrDefault(r => r.User == user);
        if (rating != null)
        {
            rating.Score = newScore;
        }
        else
        {
            Console.WriteLine("No rating from this user found.");
        }
    }

    public double ViewRating(Recipe recipe)
    {
        if (recipe.Ratings.Count == 0)
        {
            Console.WriteLine("No ratings available for this recipe.");
            return 0;
        }

        double totalScore = recipe.Ratings.Sum(r => r.Score);
        double averageScore = totalScore / recipe.Ratings.Count;
        return averageScore;
    }
}