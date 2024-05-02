namespace Recipes;
using Users;

public class Recipe
{
    private string name;
    public string Name
    {
        get { return name; }
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Name cannot be null or empty.");
            name = value;
        }
    }

    private User _owner;
    public User Owner
    {
        get { return _owner; }
        set
        {
            if (value == null)
                throw new ArgumentException("Owner cannot be null.");
            _owner = value;
        }
    }

    private string shortDescription;
    public string ShortDescription
    {
        get { return shortDescription; }
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("ShortDescription cannot be null or empty.");
            shortDescription = value;
        }
    }

    private TimeSpan preparationTime;
    public TimeSpan PreparationTime
    {
        get { return preparationTime; }
        set
        {
            if (value.TotalMinutes <= 0)
                throw new ArgumentException("PreparationTime must be greater than 0.");
            preparationTime = value;
        }
    }

    private TimeSpan cookingTime;
    public TimeSpan CookingTime
    {
        get { return cookingTime; }
        set
        {
            if (value.TotalMinutes <= 0)
                throw new ArgumentException("CookingTime must be greater than 0.");
            cookingTime = value;
        }
    }

    public TimeSpan TotalTime => PreparationTime + CookingTime;

    private int servings;
    public int Servings
    {
        get { return servings; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Servings must be greater than 0.");
            servings = value;
        }
    }
    public List<string> Steps { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Rating> Ratings { get; set; }
    public List<string> Tags { get; set; }

    public static List<string> GetSteps()
    {
        List<string> steps = new List<string>();
        Console.WriteLine("Enter cooking steps (type 'done' to finish):");
        string step;
        while ((step = Console.ReadLine().ToLower()) != "done")
        {
            steps.Add(step);
        }
        return steps;
    }

    public static List<string> GetTags()
    {
        List<string> tags = new List<string>();
        Console.WriteLine("Enter tags (type 'done' to finish):");
        string tag;
        while ((tag = Console.ReadLine().ToLower()) != "done")
        {
            tags.Add(tag);
        }
        return tags;
    }

    public override string ToString()
    {
        return $"Recipe Name: {name}, Description: {ShortDescription}, Total Time: {TotalTime}";
    }
}