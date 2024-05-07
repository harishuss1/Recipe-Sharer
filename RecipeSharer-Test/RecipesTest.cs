using Recipes;
using Users;
namespace RecipeSharerTests;


[TestClass]
public class RecipeTests
{
    [TestMethod]
        public void Constructor_ValidData_CreatesRecipeObject()
        {
            // Arrange
            User owner = new User("testperson", "testpwd123");
            string name = "Chocolate Cake";
            string description = "Delicious chocolate cake recipe";
            TimeSpan prepTime = TimeSpan.FromMinutes(30);
            TimeSpan cookingTime = TimeSpan.FromMinutes(45);
            int servings = 8;

            // Act
            Recipe recipe = new Recipe(owner, name, description, new List<Ingredient>(), prepTime, cookingTime, servings);

            // Assert
            Assert.AreEqual(owner, recipe.Owner);
            Assert.AreEqual(name, recipe.Name);
            Assert.AreEqual(description, recipe.ShortDescription);
            Assert.AreEqual(prepTime, recipe.PreparationTime);
            Assert.AreEqual(cookingTime, recipe.CookingTime);
            Assert.AreEqual(prepTime + cookingTime, recipe.TotalTime);
            Assert.AreEqual(servings, recipe.Servings);
            Assert.IsNotNull(recipe.Ingredients);
            Assert.IsNotNull(recipe.Ratings);
            Assert.IsNotNull(recipe.Tags);
        }
    [TestMethod]
    public void PropertiesTestSetAndGetCorrectly()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        Recipe recipe = new Recipe(owner, "Spaghetti Bolognese", "Authentic spaghetti bolognese recipe", new List<Ingredient>(), TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10), 4);

        // Act
        recipe.Name = "Updated Name";
        recipe.ShortDescription = "Updated description";
        recipe.PreparationTime = TimeSpan.FromMinutes(15);
        recipe.CookingTime = TimeSpan.FromMinutes(20);
        recipe.Servings = 6;

        // Assert
        Assert.AreEqual("Updated Name", recipe.Name);
        Assert.AreEqual("Updated description", recipe.ShortDescription);
        Assert.AreEqual(TimeSpan.FromMinutes(15), recipe.PreparationTime);
        Assert.AreEqual(TimeSpan.FromMinutes(20), recipe.CookingTime);
        Assert.AreEqual(TimeSpan.FromMinutes(15 + 20), recipe.TotalTime);
        Assert.AreEqual(6, recipe.Servings);
    }

    [TestMethod]
    public void TotalTime_CalculatedCorrectly()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        TimeSpan prepTime = TimeSpan.FromMinutes(30);
        TimeSpan cookingTime = TimeSpan.FromMinutes(45);

        // Act
        Recipe recipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), prepTime, cookingTime, 8);

        // Assert
        Assert.AreEqual(prepTime + cookingTime, recipe.TotalTime);
    }
    [TestMethod]
        public void Name_SetEmpty_ThrowsArgumentException()
        {
            // Arrange
            User owner = new User("testperson", "testpwd123");
            Recipe recipe = new Recipe(owner, "Spaghetti Bolognese", "Authentic spaghetti bolognese recipe", new List<Ingredient>(), TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10), 4);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => recipe.Name = "");
        }
                [TestMethod]
        public void Owner_SetNull_ThrowsArgumentException()
        {
            // Arrange
            User owner = new User("testperson", "testpwd123");
            Recipe recipe = new Recipe(owner, "Spaghetti Bolognese", "Authentic spaghetti bolognese recipe", new List<Ingredient>(), TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10), 4);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => recipe.Owner = null);
        }

        [TestMethod]
        public void PreparationTime_SetZero_ThrowsArgumentException()
        {
            // Arrange
            User owner = new User("testperson", "testpwd123");
            Recipe recipe = new Recipe(owner, "Spaghetti Bolognese", "Authentic spaghetti bolognese recipe", new List<Ingredient>(), TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10), 4);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => recipe.PreparationTime = TimeSpan.Zero);
        }

        [TestMethod]
        public void CookingTime_SetZero_ThrowsArgumentException()
        {
            // Arrange
            User owner = new User("testperson", "testpwd123");
            Recipe recipe = new Recipe(owner, "Spaghetti Bolognese", "Authentic spaghetti bolognese recipe", new List<Ingredient>(), TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10), 4);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => recipe.CookingTime = TimeSpan.Zero);
        }

        [TestMethod]
        public void Servings_SetZero_ThrowsArgumentException()
        {
            // Arrange
            User owner = new User("testperson", "testpwd123");
            Recipe recipe = new Recipe(owner, "Spaghetti Bolognese", "Authentic spaghetti bolognese recipe", new List<Ingredient>(), TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10), 4);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => recipe.Servings = 0);
        }
}

