namespace Recipes;
using Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

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

    [NotMapped]
    public ObservableCollection<Step> OSteps {get {
        if (Steps == null){
            return new();
        }
        return new(Steps);
    }}
    public List<Ingredient> Ingredients { get; set; }

    [NotMapped]
    public ObservableCollection<Ingredient> OIngredients {get {
        if (Ingredients == null){
            return new();
        }
        return new(Ingredients);
    }}
    public List<Rating> Ratings { get; set; }

    [NotMapped]
    public double ORating {get {
        return RatingOperations.INSTANCE!.ViewRating(this);
    }}

    // [InverseProperty("TaggedRecipes")]
    public List<Tag> Tags { get; set; }

    [NotMapped]
    public ObservableCollection<Tag> OTags {get {
        if (Tags == null){
            return new();
        }
        return new(Tags);
    }}

    [InverseProperty("UserFavouriteRecipes")]
    public List<User> FavoritedBy {get; set;}

    public Recipe(){
    }
    public Recipe(User owner, string name, string shortDescription, List<Ingredient> ingredients, TimeSpan preparationTime, TimeSpan cookingTime, int servings)
{
    Owner = owner;
    Name = name;
    ShortDescription = shortDescription;
    Ingredients = ingredients ?? new List<Ingredient>();
    PreparationTime = preparationTime;
    CookingTime = cookingTime;
    Servings = servings;
    Steps = new List<Step>();
    Ratings = new List<Rating>();
    Tags = new List<Tag>();
    FavoritedBy = new List<User>();
}


    public override string ToString()
    {
        return $"Recipe Name: {name}, Description: {ShortDescription}, Total Time: {TotalTime}";
    }
}