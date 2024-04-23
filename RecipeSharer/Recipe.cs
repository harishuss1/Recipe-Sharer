namespace Recipes;
using Users;

public class Recipe
{
    private User user;
    private string v1;
    private string v2;
    private int v3;
    private int v4;
    private int v5;

    public string Name { get; set; }
    public User Owner { get; set; }
    public string ShortDescription { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public TimeSpan CookingTime { get; set; }
    public TimeSpan TotalTime => PreparationTime + CookingTime;
    public int Servings { get; set; }
    public List<string> Steps { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Rating> Ratings { get; set; }
    public List<string> Tags { get; set; }

    public Recipe(User owner, string name, string description, List<Ingredient> ingredients,TimeSpan prepTime, TimeSpan cookingTime, int servings)
    {
        Owner = owner;
        Name = name;
        ShortDescription = description;
        PreparationTime = prepTime;
        CookingTime = cookingTime;
        Servings = servings;
        Ingredients = ingredients;
        Steps = new List<string>();
        Ratings = new List<Rating>();
        Tags = new List<string>();
    }

    // Implement recipe creation, update, deletion, and rating logic as methods here
}