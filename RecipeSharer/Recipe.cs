namespace Recipes;
using Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Recipe
{
    [Key]
    public int RecipeId {get;set;}
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
    
    [ForeignKey("OwnerId")]
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
    public List<Step> Steps { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Rating> Ratings { get; set; }

    // [InverseProperty("TaggedRecipes")]
    public List<Tag> Tags { get; set; }

    //[InverseProperty("UserFavouriteRecipes")]
    public List<User> FavoritedBy {get; set;}

    public Recipe(){
    }

    public static List<Step> GetSteps()
    {
        List<Step> steps = new List<Step>();
        Console.WriteLine("Enter cooking steps (type 'done' to finish):");
        string step;
        while ((step = Console.ReadLine().ToLower()) != "done")
        {
            Step s = new Step();
            s.Description = step;
            steps.Add(s);
        }
        return steps;
    }
    public static List<Tag> GetTags()
    {
        List<Tag> tags = new List<Tag>();
        Console.WriteLine("Enter tags (type 'done' to finish):");
        string tag;
        while ((tag = Console.ReadLine().ToLower()) != "done")
        {
            Tag t = new Tag();
            t.Name = tag;
            tags.Add(t);
        }
        return tags;
    }

    public override string ToString()
    {
        return $"Recipe Name: {name}, Description: {ShortDescription}, Total Time: {TotalTime}";
    }
}