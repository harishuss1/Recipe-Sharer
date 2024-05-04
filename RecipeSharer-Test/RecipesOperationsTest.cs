namespace RecipeSharerTests;
using Recipes;
using System;
using Users;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;



[TestClass]
public class RecipeOperationsTests
{

    private User _user;
    private Recipe _recipe;

    private static (Mock<RecipeSharerContext>, Mock<DbSet<Recipe>>) GetMocks()
    {
        var mockContext = new Mock<RecipeSharerContext>();
        var mockRecipes = new Mock<DbSet<Recipe>>();
        mockContext.Setup(mock => mock.Recipes).Returns(mockRecipes.Object);

        return (mockContext, mockRecipes);
    }

    private static void ConfigureDbSetMock<T>(IQueryable<T> data, Mock<DbSet<T>> mockDbSet) where T : class
    {
        mockDbSet.As<IQueryable<T>>().Setup(mock => mock.Provider).Returns(data.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(mock => mock.Expression).Returns(data.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(mock => mock.ElementType).Returns(data.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User
        {
            UserId = 1,
            Username = "Test User"
        };

        _recipe = new Recipe
        {
            RecipeId = 1,
            Name = "Test Recipe"
        };
    }



    [TestMethod]
    public void AddRecipe_ValidInputs_ShouldAddRecipe()
    {
        // Arrange
        var recipes = new List<Recipe>().AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        // Act
        recipeOperations.AddRecipe(_user, _recipe);

        // Assert
        mockRecipes.Verify(m => m.Add(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }



    [TestMethod]
    public void RemoveRecipe_RecipeExists_ShouldRemoveRecipe()
    {
        // Arrange
        var recipes = new List<Recipe>
    {
        new Recipe { Owner = _user, Name = "Test Recipe" }
    }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        // Act
        recipeOperations.RemoveRecipe(_user, _recipe);

        // Assert
        mockRecipes.Verify(m => m.Remove(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }


    // Test for UpdateRecipe method
    [TestMethod]
    public void UpdateRecipeTestUpdatesExistingRecipeWithValidDetails()
    {
        // Arrange
        var recipes = new List<Recipe>
    {
        new Recipe { Owner = _user, Name = "Chocolate Cake" }
    }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        Recipe existingRecipe = new Recipe(_user, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        Recipe newDetails = new Recipe(_user, "Vanilla Cake", "Delicious vanilla cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act
        recipeOperations.AddRecipe(_user, existingRecipe);
        recipeOperations.UpdateRecipe(_user, existingRecipe, newDetails);

        // Assert
        Assert.AreEqual(newDetails.Name, existingRecipe.Name);
        Assert.AreEqual(newDetails.ShortDescription, existingRecipe.ShortDescription);
        Assert.AreEqual(newDetails.PreparationTime, existingRecipe.PreparationTime);
        Assert.AreEqual(newDetails.CookingTime, existingRecipe.CookingTime);
        Assert.AreEqual(newDetails.TotalTime, existingRecipe.TotalTime);
        Assert.AreEqual(newDetails.Servings, existingRecipe.Servings);
        CollectionAssert.AreEqual(newDetails.Steps, existingRecipe.Steps, "The steps do not match.");
        CollectionAssert.AreEqual(newDetails.Ingredients, existingRecipe.Ingredients, "The ingredients do not match.");
        CollectionAssert.AreEquivalent(newDetails.Tags, existingRecipe.Tags, "The tags do not match.");

        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    // next 3 methods, I think I would need to call the update method...? but it will fail before it reaches the end of the test soo????
    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentExceptionWhenNewOwnerIsDifferent()
    {
        // Arrange
        var recipes = new List<Recipe>
    {
        new Recipe { Owner = _user, Name = "Chocolate Cake" }
    }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        User owner = new User("testperson", "testpwd123");
        User owner2 = new User("testperson2", "testpwd123");
        Recipe existingRecipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        Recipe newDetails = new Recipe(owner2, "Vanilla Cake", "Delicious vanilla cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.UpdateRecipe(owner, existingRecipe, newDetails));

        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentNullExceptionWhenExistingRecipeIsNull()
    {
        // Arrange
        var recipes = new List<Recipe>().AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        User owner = new User("testperson", "testpwd123");
        Recipe newDetails = new Recipe(owner, "Vanilla Cake", "Delicious vanilla cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act and Assert
        Assert.ThrowsException<ArgumentNullException>(() => recipeOperations.UpdateRecipe(owner, null, newDetails));

        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentNullExceptionWhenNewDetailsIsNull()
    {
        // Arrange
        var recipes = new List<Recipe>().AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        User owner = new User("testperson", "testpwd123");
        Recipe existingRecipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act and Assert
        Assert.ThrowsException<ArgumentNullException>(() => recipeOperations.UpdateRecipe(owner, existingRecipe, null));

        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsSteps()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var steps = new List<Step> { new Step { Description = "Step 1" }, new Step { Description = "Step 2" } };

        // Act
        recipeOperations.AddStepsToRecipe(1, steps);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsEmptyListWhenNoStepsEntered()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var steps = new List<Step>();

        // Act
        recipeOperations.AddStepsToRecipe(1, steps);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsStepsWhenSingleStepEntered()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var steps = new List<Step> { new Step { Description = "Step 1" } };

        // Act
        recipeOperations.AddStepsToRecipe(1, steps);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsStepsIgnoresLeadingAndTrailingSpaces()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var steps = new List<Step> { new Step { Description = "  Step 1  " }, new Step { Description = "  Step 2  " } };
        var expectedSteps = new List<Step> { new Step { Description = "Step 1" }, new Step { Description = "Step 2" } };

        // Act
        recipeOperations.AddStepsToRecipe(1, steps);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void AddTagsToRecipeThrowsArgumentNullExceptionWhenTagsIsNull()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        // Act and Assert
        Assert.ThrowsException<ArgumentNullException>(() => recipeOperations.AddTagsToRecipe(1, null));

        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Never);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void AddTagsToRecipeThrowsArgumentExceptionWhenRecipeNotFound()
    {
        // Arrange
        var recipes = new List<Recipe>().AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var tags = new List<Tag> { new Tag { Name = "Tag 1" } };

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.AddTagsToRecipe(1, tags));

        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void AddTagsToRecipeAddsTagsToRecipe()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var tags = new List<Tag> { new Tag { Name = "Tag 1" }, new Tag { Name = "Tag 2" } };

        // Act
        recipeOperations.AddTagsToRecipe(1, tags);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void AddIngredientThrowsArgumentNullExceptionWhenIngredientIsNull()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        // Act and Assert
        Assert.ThrowsException<ArgumentNullException>(() => recipeOperations.addIngredient(1, null));

        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Never);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void AddIngredientThrowsArgumentExceptionWhenRecipeNotFound()
    {
        // Arrange
        var recipes = new List<Recipe>().AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var ingredient = new Ingredient { Name = "Ingredient 1" };

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.addIngredient(1, ingredient));

        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void AddIngredientAddsIngredientToRecipe()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var ingredient = new Ingredient { Name = "Ingredient 1" };

        // Act
        recipeOperations.addIngredient(1, ingredient);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }
}
