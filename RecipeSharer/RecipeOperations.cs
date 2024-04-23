using RecipeSharer;

namespace Recipes;
using Users;
public class RecipeOperations
{

    // Not sure if i should manage everything locally or from the database
    // Will be discuseed among teamates

    // will need a ViewRecipe method? with data pulled from the db 

    //Also might will be using Our Validation class after when refactoring code.
    public List<Recipe> recipes;

    public RecipeOperations()
    {
        recipes = new List<Recipe>(); // pulled from database
        // will be info taken from the db? Is it this method needed? can just do line 13 as equality to line 9 no?
    }

    // Add a new recipe
    public void AddRecipe(User user,Recipe recipe)
    {
        if (recipe.Owner == null)
            throw new ArgumentException("Recipe must have an owner.");
        if (string.IsNullOrEmpty(recipe.Name))
        {
            throw new ArgumentException("Recipe name cannot be null or empty.", nameof(recipe.Name));
        }
        recipe.Owner= user;
        recipes.Add(recipe);
    }

    // Remove a recipe
    public void RemoveRecipe(User user, Recipe recipe)
{
    if (recipe.Owner == null)
        throw new ArgumentException("Recipe must have an owner.");
    if (recipe.Owner != user)
        throw new ArgumentException("Only the owner can remove the recipe.");
    recipes.Remove(recipe);
}

    // Update a recipe
    public void UpdateRecipe(User user,Recipe existingRecipe, Recipe newDetails)
    {
        if (existingRecipe.Owner != newDetails.Owner || existingRecipe.Owner != user)
        throw new ArgumentException("Only the owner can update the recipe.");
        
        if (existingRecipe.Owner != newDetails.Owner)
            throw new ArgumentException("Cannot change the owner of the recipe.");

        existingRecipe.Name = newDetails.Name;
        existingRecipe.ShortDescription = newDetails.ShortDescription;
        existingRecipe.Ingredients = newDetails.Ingredients;
        existingRecipe.PreparationTime = newDetails.PreparationTime;
        existingRecipe.CookingTime = newDetails.CookingTime;
        existingRecipe.Servings = newDetails.Servings;
        existingRecipe.Steps = new List<string>(newDetails.Steps);
        existingRecipe.Tags = new List<string>(newDetails.Tags);
    }

    // add steps to a recipe
    public List<string> AddStepsToRecipe(TextReader reader)
    {
    var steps = new List<string>();
    string step;
    while ((step = reader.ReadLine()) != null)
    {
        if (step.Trim().Equals("Done!", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }

        steps.Add(step.Trim());
    }
    return steps;
    }

    // add ingredient to recipe
    public void addIngredient(Recipe recipe, Ingredient ingredient){
        recipe.Ingredients.Add(ingredient);
    }

    //View all recipes
    public void ViewRecipes(){
        int count = 0;
        foreach (Recipe recipe in recipes){
                count++;
                Console.WriteLine($"{count}: {recipe.ToString()}");
        }
        if (count == 0){
            Console.WriteLine("No Recipes Found");
        }
    }

    //View user's recipe lists
    public void ViewUserRecipes(User owner){
        int count = 0;
        foreach (Recipe recipe in recipes){
            if (recipe.Owner == owner){
                count++;
                Console.WriteLine($"{count}: {recipe.ToString()}");
            }
        }
        if (count == 0){
            Console.WriteLine("No Recipes Found");
        }
    }

    public List<Recipe> GetUserRecipes(User owner){
        List<Recipe> r = new List<Recipe>();
        foreach (Recipe recipe in recipes){
            if (recipe.Owner == owner){
                r.Add(recipe);
            }
        }
        return r;
    }
}
