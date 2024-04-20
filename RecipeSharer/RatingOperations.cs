namespace Recipe;
using User;

public class RatingOperations
{
    public void AddRating(User user, Recipe recipe)
    {
        while (true)
        {
            Console.WriteLine("Please enter a rating between 0 and 10:");
            string input = Console.ReadLine();
            try
            {
                int score = int.Parse(input);
                Rating rating = new Rating(user, score); // Trying to make a recipe.
                recipe.Ratings.Add(rating); 
                break; // We exit the loop if we succefully reached this line, meaning we added the rating.
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid number. Please try again.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Rating must be between 0 and 10. Please try again.");
            }
        }
    }

    //Missing Remove, Update Operations.
}
