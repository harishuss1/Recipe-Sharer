using Recipes;
using RecipeSearch;
using Users;
namespace RecipeSharerTests;

[TestClass]
    public class SearchTests
    {
        private Search _search;
        private List<Recipe> _recipes;
        private User _testUser;


        [TestInitialize]
        public void Initialize()
        {
            _search = new Search();
            _testUser = new User("ChefGourmet", "password123");  

            _recipes = new List<Recipe>
            {
                new Recipe(_testUser, "Chocolate Cake", "Delicious dark chocolate cake", new List<Ingredient>(), TimeSpan.FromMinutes(20), TimeSpan.FromHours(1), 4) 
                {
                    Ingredients = new List<Ingredient> { new Ingredient("Chocolate", 200, "g", "Solid") },
                    Tags = new List<string> { "Dessert" },
                    Ratings = new List<Rating> { new Rating(_testUser, 5) }  
                },
                new Recipe(_testUser, "Tomato Soup", "Fresh and creamy tomato soup", new List<Ingredient>(), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(30), 2) 
                {
                    Ingredients = new List<Ingredient> { new Ingredient("Tomato", 5, "pcs", "Vegetable") },
                    Tags = new List<string> { "Soup", "Vegan" },
                    Ratings = new List<Rating> { new Rating(_testUser, 4) }  
                }
            };
        }

        [TestMethod]
        public void AddTag_ValidTag_ShouldAddTag()
        {
            _search.AddTag("Vegan");
            Assert.IsTrue(_search.Tags.Contains("Vegan"));
        }

        [TestMethod]
        public void SetIngredients_ValidIngredients_ShouldSetIngredients()
        {
            var ingredients = new List<string> { "Chocolate", "Tomato" };
            _search.SetIngredients(ingredients);
            Assert.IsTrue(_search.Ingredients.SequenceEqual(ingredients));
        }

        [TestMethod]
        public void SetKeyword_ValidKeyword_ShouldSetKeyword()
        {
            var keyword = "Cake";
            _search.SetKeyword(keyword);
            Assert.AreEqual(keyword, _search.Keyword);
        }

        [TestMethod]
        public void SetTimeConstraints_ValidTimes_ShouldSetTimeConstraints()
        {
            var minDuration = TimeSpan.FromMinutes(20);
            var maxDuration = TimeSpan.FromHours(2);
            _search.SetTimeConstraints(minDuration, maxDuration);
            Assert.AreEqual(minDuration, _search.MinDuration);
            Assert.AreEqual(maxDuration, _search.MaxDuration);
        }

        [TestMethod]
        public void SetServingsConstraints_ValidServings_ShouldSetServingsConstraints()
        {
            _search.SetServingsConstraints(1, 10);
            Assert.AreEqual(1, _search.MinServings);
            Assert.AreEqual(10, _search.MaxServings);
        }

        [TestMethod]
        public void SetMinimumRating_ValidRating_ShouldSetMinimumRating()
        {
            _search.SetMinimumRating(3.5);
            Assert.AreEqual(3.5, _search.MinimumRating);
        }

        [TestMethod]
        public void PerformSearch_WithCriteria_ShouldReturnFilteredRecipes()
        {
            _search.SetIngredients(new List<string> { "Chocolate" });
            _search.AddTag("Dessert");
            _search.SetKeyword("Cake");

            var result = _search.PerformSearch(_recipes);

            Assert.IsTrue(result.Any(), "Should return at least one recipe.");
            Assert.IsTrue(result.All(r => r.Name.Contains("Chocolate Cake")), "Should return recipes containing 'Chocolate Cake'.");
        }
    }