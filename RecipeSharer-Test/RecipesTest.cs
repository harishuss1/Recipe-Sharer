namespace RecipeSharer.Tests
{
    public class RecipeTests
    {
        [TestMethod]
        public void ConstructorTestWithValidDataAndReturnsValidRecipeObject()
        {
            // Arrange
            User owner = new User("testperson", "testpwd");
            string name = "Chocolate Cake";
            string description = "Delicious chocolate cake recipe";
            TimeSpan prepTime = TimeSpan.FromMinutes(30);
            TimeSpan cookingTime = TimeSpan.FromMinutes(45);
            int servings = 8;

            // Act
            Recipe recipe = new Recipe(owner, name, description, prepTime, cookingTime, servings);

            // Assert
            // should I do 1 test for each assert...?
            Assert.Equals(owner, recipe.Owner);
            Assert.Equals(name, recipe.Name);
            Assert.Equals(description, recipe.ShortDescription);
            Assert.Equals(prepTime, recipe.PreparationTime);
            Assert.Equals(cookingTime, recipe.CookingTime);
            Assert.Equals(prepTime + cookingTime, recipe.TotalTime);
            Assert.Equals(servings, recipe.Servings);
            Assert.IsNotNull(recipe.Steps);
            Assert.IsNotNull(recipe.Ingredients);
            Assert.IsNotNull(recipe.Ratings);
            Assert.IsNotNull(recipe.Tags);
        }

        [TestMethod]
        public void PropertiesTestSetAndGetCorrectly()
        {
            // Arrange
            User owner = new User("testperson", "testpwd");
            Recipe recipe = new Recipe(owner, "Spaghetti Bolognese", "Authentic spaghetti bolognese recipe", TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10), 4);

            // Act
            recipe.Name = "Updated Name";
            recipe.ShortDescription = "Updated description";
            recipe.PreparationTime = TimeSpan.FromMinutes(15);
            recipe.CookingTime = TimeSpan.FromMinutes(20);
            recipe.Servings = 6;

            // Assert
            Assert.Equals("Updated Name", recipe.Name);
            Assert.Equals("Updated description", recipe.ShortDescription);
            Assert.Equals(TimeSpan.FromMinutes(15), recipe.PreparationTime);
            Assert.Equals(TimeSpan.FromMinutes(20), recipe.CookingTime);
            Assert.Equals(TimeSpan.FromMinutes(15 + 20), recipe.TotalTime);
            Assert.Equals(6, recipe.Servings);
        }

        [TestMethod]
        public void TotalTime_CalculatedCorrectly()
        {
            // Arrange
            User owner = new User("testperson", "testpwd");
            TimeSpan prepTime = TimeSpan.FromMinutes(30);
            TimeSpan cookingTime = TimeSpan.FromMinutes(45);

            // Act
            Recipe recipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", prepTime, cookingTime, 8);

            // Assert
            Assert.Equals(prepTime + cookingTime, recipe.TotalTime);
        }

    }
}
