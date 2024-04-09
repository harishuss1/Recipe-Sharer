public class Recipes
{
// recipe name property 
    public string RecipeName { get; set; }
// description property
    public string RecipeDesc { get; set; }
// ingredients property
    public List<string> RecipeIngredients { get; }
// owner property
    public string RecipeOwner { get; set; }
// instructions property
    public string RecipeInstructions { get; set; }
// rating property
    public int RecipeRating { get; set; }
// tags
    public List<string> RecipeTag { get; set;}
// prep time
    public int PrepTime { get; set; }
// cooking time
    public int CookingTime { get; set; }
// servings
    public int NumOfServings {get;}

// addRecipe

// removeRecipe

// modifyRecipe (the description, name, ingredients...)

// rateRecipe (user, rating)
}