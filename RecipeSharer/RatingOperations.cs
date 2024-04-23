namespace Recipes;
using Users;

public class RatingOperations
{
    public void AddRating(User user, Recipe recipe)
    {
        if (user == null || recipe == null)
        {
            Console.WriteLine("User or Recipe cannot be null.");
            return;
        }

        while (true)
        {
            Console.WriteLine("Please enter a rating between 0 and 10:");
            string input = Console.ReadLine();
            try
            {
                int score = int.Parse(input);
                if (score < 0 || score > 10)
                {
                    Console.WriteLine("Rating must be between 0 and 10. Please try again.");
                    continue;
                }
                Rating rating = new Rating(user, score); // Trying to make a recipe.
                recipe.Ratings.Add(rating); 
                break; // We exit the loop if we successfully reached this line, meaning we added the rating.
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid number. Please try again.");
            }
        }
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
}
