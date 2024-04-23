using RecipeSharer;

namespace Recipes;

public class RecipeOperations
{

    // Not sure if i should manage everything locally or from the database
    // Will be discuseed among teamates

    //Also might will be using Our Validation class after when refactoring code.
    private List<Recipe> recipes;

    public RecipeOperations()
    {
        recipes = new List<Recipe>(); // pulled from database
        // will be info taken from the db? Is it this method needed? can just do line 13 as equality to line 9 no?
    }

    // Add a new recipe
    public void AddRecipe(Recipe recipe)
    {
        if (recipe.Owner == null)
            throw new ArgumentException("Recipe must have an owner.");

        recipes.Add(recipe);
    }

    // Remove a recipe
    public void RemoveRecipe(Recipe recipe)
    {
        recipes.Remove(recipe);
    }

    // Update a recipe
    public void UpdateRecipe(Recipe existingRecipe, Recipe newDetails)
    {
        if (existingRecipe.Owner != newDetails.Owner)
            throw new ArgumentException("Cannot change the owner of the recipe.");

        existingRecipe.Name = newDetails.Name;
        existingRecipe.ShortDescription = newDetails.ShortDescription;
        existingRecipe.PreparationTime = newDetails.PreparationTime;
        existingRecipe.CookingTime = newDetails.CookingTime;
        existingRecipe.Servings = newDetails.Servings;
        existingRecipe.Steps = new List<string>(newDetails.Steps);
        existingRecipe.Ingredients = new List<Ingredient>(newDetails.Ingredients);
        existingRecipe.Tags = new List<string>(newDetails.Tags);
    }

    // add steps to a recipe
    public List<string> AddStepsToRecipe()
    {
        Console.WriteLine("Add steps to the recipe. Press Enter after each step. Type 'Done!' when finished:");

        List<string> steps = new List<string>();
        string step;
        while (true)
        {
            step = Console.ReadLine();
            if (step.ToLower() == "done!")
            {
                break;
            }
            else
            {
                steps.Add(step);
            }
        }
        return steps;
    }
}
