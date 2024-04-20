namespace Recipe;
using User;
public class Recipe
{
    public string Name { get; set; }
    public User Owner { get; set; }
    public string ShortDescription { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public TimeSpan CookingTime { get; set; }
    public TimeSpan TotalTime { get; set; }
    public int Servings { get; set; }
    public List<string> Steps { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Rating> Ratings { get; set; }
    public List<string> Tags { get; set; }

    public Recipe(User owner, string name)
    {
        Owner = owner;
        Name = name;
        Steps = new List<string>();
        Ingredients = new List<Ingredient>();
        Ratings = new List<Rating>();
        Tags = new List<string>();
    }

    // Implement recipe creation, update, deletion, and rating logic as methods here
}
