
namespace RecipeSharer;
class Program
{
    public static void Main(string[] args)
    {
        // Test ingredients with different quantity formats
        List<Ingredient> originalIngredients = new List<Ingredient>()
        {
            new Ingredient("Flour", 200, "g", "flour"),
            new Ingredient("Sugar", 100, "g", "Sugar"),
            new Ingredient("Eggs", 3, "units", "eggs"),
            new Ingredient("Milk", 150, "ml", "dairy")
            // Add more ingredients as needed
        };

        // Scale all ingredients
        foreach (var ingredient in originalIngredients)
        {
            ingredient.recipeScaler();
        }
    }

}