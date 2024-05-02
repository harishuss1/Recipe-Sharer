namespace Utils;
using Recipes;
using Users;
public class ConsoleUtils
{
    public int GetValidRatingFromUser()
    {
        int rating;
        do
        {
            Console.Write("Please enter a rating from 0 to 10: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out rating) && rating >= 0 && rating <= 10)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid rating. Please enter a number from 1 to 10.");
            }
        } while (true);

        return rating;
    }
    public List<Step> GetStepsFromUser()
    {
        var steps = new List<Step>();
        string input;
        int stepNumber = 1;
        while ((input = Console.ReadLine()) != "Done!")
        {
            steps.Add(new Step { Number = stepNumber, Description = input });
            stepNumber++;
        }

        return steps;
    }

    public List<Tag> GetTagsFromUser()
    {
        var tags = new List<Tag>();
        string input;
        while ((input = Console.ReadLine()) != "Done!")
        {
            tags.Add(new Tag { Name = input });
        }

        return tags;
    }

    public static Recipe GetValidRecipe(User currentUser)
    {
        Console.WriteLine("Please enter the recipe details:");

        // Get name
        Console.Write("Recipe name: ");
        string name = ReadNonEmptyString("Recipe name cannot be empty.");

        // Get short description
        Console.Write("Short description: ");
        string description = ReadNonEmptyString("Short description cannot be empty.");

        // Get preparation time
        Console.Write("Preparation time in minutes: ");
        TimeSpan prepTime = TimeSpan.FromMinutes(ReadValidInteger("Please enter a valid preparation time in minutes."));

        // Get cooking time
        Console.Write("Cooking time in minutes: ");
        TimeSpan cookTime = TimeSpan.FromMinutes(ReadValidInteger("Please enter a valid cooking time in minutes."));

        // Get servings
        Console.Write("Number of servings: ");
        int servings = ReadValidInteger("Please enter a valid number of servings.");

        // Get ingredients
        List<Ingredient> ingredients = GetIngredients();

        // Get steps
        List<Step> steps = Recipe.GetSteps();

        // Get tags
        List<Tag> tags = Recipe.GetTags();

        // Create and return the recipe object
        Recipe r = new() { Owner = currentUser, Name = name, ShortDescription = description, Ingredients = ingredients, PreparationTime = prepTime, CookingTime = cookTime, Servings = servings, Steps = steps, Tags = tags };
        return r;
    }

    public static string ReadNonEmptyString(string errorMessage)
    {
        string input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine(errorMessage);
            input = Console.ReadLine();
        }
        return input;
    }

    public static int ReadValidInteger(string errorMessage)
    {
        int number;
        while (!int.TryParse(Console.ReadLine(), out number) || number <= 0)
        {
            Console.WriteLine(errorMessage);
        }
        return number;
    }

    private static List<Ingredient> GetIngredients()
    {
        List<Ingredient> ingredients = new List<Ingredient>();
        Console.WriteLine("Enter ingredients (type 'done' to finish):");
        string input;
        while ((input = Console.ReadLine().ToLower()) != "done")
        {
            // Assuming the existence of a method to parse ingredient strings
            ingredients.Add(ParseIngredient(input));
        }
        return ingredients;
    }


    private static Ingredient ParseIngredient(string input)
    {
        // Example parsing logic, adjust as necessary for your Ingredient class
        return new Ingredient() { Name = input, Quantity = 1, UnitOfMass = "unit", Type = null }; // Placeholder, adjust constructor as necessary
    }

}