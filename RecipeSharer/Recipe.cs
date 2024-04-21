using System;
using System.Collections.Generic;
namespace RecipeSharer;
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

    public Recipe(User owner, string name, string description, TimeSpan prepTime, TimeSpan cookingTime, int servings)
    {
        Owner = owner;
        Name = name;
        ShortDescription = description;
        PreparationTime = prepTime;
        CookingTime = cookingTime;
        TotalTime = prepTime + cookingTime;
        Servings = servings;
        Steps = new List<string>();
        Ingredients = new List<Ingredient>();
        Ratings = new List<Rating>();
        Tags = new List<string>();
    }

    // Implement recipe creation, update, deletion, and rating logic as methods here
}
