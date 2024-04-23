using Recipes;
using RecipeSearch;
using Users;
namespace RecipeSharerTests;
[TestClass]
public class SearchTests
{
    private Search _search;
    private List<Recipe> _recipes;
    private User _user;

    [TestInitialize]
    public void TestInitialize()
    {
        _search = new Search();
        _user = new User("ChefGourmet", "password123");

        _recipes = new List<Recipe>
            {
                new Recipe(_user, "Apple Pie")
                {
                    Ingredients = new List<Ingredient> { new Ingredient("Apple", "2 pieces", "Fruit"), new Ingredient("Sugar", "100g", "Condiment") },
                    Tags = new List<string> { "Dessert", "Baking" },
                    PreparationTime = TimeSpan.FromMinutes(30),
                    CookingTime = TimeSpan.FromMinutes(60),
                    Servings = 4
                },
                new Recipe(_user, "Tomato Soup")
                {
                    Ingredients = new List<Ingredient> { new Ingredient("Tomato", "5 pieces", "Vegetable"), new Ingredient("Salt", "5g", "Condiment") },
                    Tags = new List<string> { "Soup", "Vegan" },
                    PreparationTime = TimeSpan.FromMinutes(10),
                    CookingTime = TimeSpan.FromMinutes(20),
                    Servings = 2
                }
            };
    }

    [TestMethod]
    public void AddTag_ValidTag_ShouldSucceed()
    {
        _search.AddTag("Dessert");
        Assert.IsTrue(_search.Tags.Contains("Dessert"));
    }

    [TestMethod]
    public void SetIngredients_ValidIngredients_ShouldSucceed()
    {
        List<string> ingredients = new List<string> { "Apple", "Tomato" };
        _search.SetIngredients(ingredients);
        Assert.IsTrue(_search.Ingredients.SequenceEqual(ingredients));
    }

    [TestMethod]
    public void SetKeyword_ValidKeyword_ShouldSucceed()
    {
        string keyword = "Pie";
        _search.SetKeyword(keyword);
        Assert.AreEqual(keyword, _search.Keyword);
    }

    [TestMethod]
    public void PerformSearch_WithCriteria_ShouldReturnExpectedResults()
    {
        _search.SetIngredients(new List<string> { "Apple" });
        _search.AddTag("Dessert");
        _search.SetKeyword("Pie");

        var results = _search.PerformSearch(_recipes);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("Apple Pie", results[0].Name);
    }

    [TestMethod]
    public void SetTimeConstraints_ValidTimes_ShouldSucceed()
    {
        _search.SetTimeConstraints(TimeSpan.FromMinutes(50), TimeSpan.FromMinutes(120));
        Assert.AreEqual(TimeSpan.FromMinutes(50), _search.MinDuration);
        Assert.AreEqual(TimeSpan.FromMinutes(120), _search.MaxDuration);
    }

    [TestMethod]
    public void SetServingsConstraints_ValidRange_ShouldSucceed()
    {
        _search.SetServingsConstraints(1, 5);
        Assert.AreEqual(1, _search.MinServings);
        Assert.AreEqual(5, _search.MaxServings);
    }

    [TestMethod]
    public void SetMinimumRating_ValidRating_ShouldSucceed()
    {
        _search.SetMinimumRating(3.5);
        Assert.AreEqual(3.5, _search.MinimumRating);
    }
}
