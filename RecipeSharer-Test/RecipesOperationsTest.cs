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

        // Initialize objectss
        var _user = new User { UserId = 1, Username = "TestUser" };
        var _recipe = new Recipe { RecipeId = 1, Name = "TestRecipe", Owner = _user };

        // Set up Users DbSet to return _user when Find is called with _user.UserId
        var mockUsers = new Mock<DbSet<User>>();
        mockUsers.Setup(m => m.Find(_user.UserId)).Returns(_user);
        mockContext.Setup(m => m.Users).Returns(mockUsers.Object);

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
        var _user = new User { UserId = 1, Username = "TestUser" };
        var _recipe = new Recipe { RecipeId = 1, Name = "TestRecipe", Owner = _user };
        var recipes = new List<Recipe> { _recipe }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        // Set up Recipes DbSet to return _recipe when Find is called with _recipe.RecipeId
        mockRecipes.Setup(m => m.Find(_recipe.RecipeId)).Returns(_recipe);

        // Act
        recipeOperations.RemoveRecipe(_user, _recipe);

        // Assert
        mockRecipes.Verify(m => m.Remove(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }


    [TestMethod]
    public void UpdateRecipeTestUpdatesExistingRecipeWithValidDetails()
    {
        // Arrange
        var _user = new User { UserId = 1, Username = "TestUser" };
        var recipes = new List<Recipe>
    {
        new Recipe { Owner = _user, Name = "Chocolate Cake" }
    }.AsQueryable();
        var users = new List<User> { _user }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        var mockUsers = new Mock<DbSet<User>>();
        mockContext.Setup(mock => mock.Users).Returns(mockUsers.Object);
        ConfigureDbSetMock(recipes, mockRecipes);
        ConfigureDbSetMock(users, mockUsers);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        // Set up Users DbSet to return _user when Find is called with _user.UserId
        mockUsers.Setup(m => m.Find(_user.UserId)).Returns(_user);

        Recipe existingRecipe = new Recipe
        {
            Owner = _user,
            Name = "Chocolate Cake",
            ShortDescription = "Delicious chocolate cake recipe",
            Ingredients = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(30),
            CookingTime = TimeSpan.FromMinutes(45),
            Servings = 8,
            Steps = new List<Step>(),
            Tags = new List<Tag>()
        };
        Recipe newDetails = new Recipe
        {
            Owner = _user,
            Name = "Vanilla Cake",
            ShortDescription = "Delicious vanilla cake recipe",
            Ingredients = new List<Ingredient>(),
            PreparationTime = TimeSpan.FromMinutes(25),
            CookingTime = TimeSpan.FromMinutes(40),
            Servings = 10,
            Steps = new List<Step>(),
            Tags = new List<Tag>()
        };
        // Act
        recipeOperations.AddRecipe(_user, existingRecipe);

        // Set up Recipes DbSet to return existingRecipe when Find is called with existingRecipe.RecipeId
        mockRecipes.Setup(m => m.Find(existingRecipe.RecipeId)).Returns(existingRecipe);

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
        mockContext.Verify(m => m.SaveChanges(), Times.Exactly(2));
    }

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
    public void UpdateRecipeTestThrowsArgumentExceptionWhenExistingRecipeIsNull()
    {
        // Arrange
        var recipes = new List<Recipe>().AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        User owner = new User("testperson", "testpwd123");
        Recipe newDetails = new Recipe(owner, "Vanilla Cake", "Delicious vanilla cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.UpdateRecipe(owner, null, newDetails));

        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentExceptionWhenNewDetailsIsNull()
    {
        // Arrange
        var recipes = new List<Recipe>().AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        User owner = new User("testperson", "testpwd123");
        Recipe existingRecipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.UpdateRecipe(owner, existingRecipe, null));

        mockRecipes.Verify(m => m.Update(It.IsAny<Recipe>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsSteps()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1, Steps = new List<Step>() } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);
        mockRecipes.Setup(m => m.Find(It.IsAny<object[]>())).Returns((object[] id) => recipes.FirstOrDefault(r => r.RecipeId == (int)id[0]));
        var steps = new List<Step> { new Step { Description = "Step 1" }, new Step { Description = "Step 2" } };

        // Act
        recipeOperations.AddStepsToRecipe(1, steps);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<object[]>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
        var recipe = recipes.First();
        Assert.AreEqual(steps.Count, recipe.Steps.Count);
        for (int i = 0; i < steps.Count; i++)
        {
            Assert.AreEqual(steps[i].Description, recipe.Steps[i].Description);
        }
    }

    [TestMethod]
    public void AddStepsToRecipeThrowsExceptionWhenNoStepsEntered()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1 } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);

        var steps = new List<Step>();

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentNullException>(() => recipeOperations.AddStepsToRecipe(1, steps));
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsStepsWhenSingleStepEntered()
    {
        // Arrange
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1, Steps = new List<Step>() } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);
        mockRecipes.Setup(m => m.Find(It.IsAny<object[]>())).Returns((object[] id) => recipes.FirstOrDefault(r => r.RecipeId == (int)id[0]));
        var steps = new List<Step> { new Step { Description = "Step 1" } };

        // Act
        recipeOperations.AddStepsToRecipe(1, steps);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<object[]>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
        var recipe = recipes.First();
        Assert.AreEqual(steps.Count, recipe.Steps.Count);
        for (int i = 0; i < steps.Count; i++)
        {
            Assert.AreEqual(steps[i].Description, recipe.Steps[i].Description);
        }
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
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1, Tags = new List<Tag>() } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);
        mockRecipes.Setup(m => m.Find(It.IsAny<object[]>())).Returns((object[] id) => recipes.FirstOrDefault(r => r.RecipeId == (int)id[0]));
        var tags = new List<Tag> { new Tag { Name = "Tag 1" }, new Tag { Name = "Tag 2" } };

        // Act
        recipeOperations.AddTagsToRecipe(1, tags);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<object[]>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
        var recipe = recipes.First();
        Assert.AreEqual(tags.Count, recipe.Tags.Count);
        for (int i = 0; i < tags.Count; i++)
        {
            Assert.AreEqual(tags[i].Name, recipe.Tags[i].Name);
        }
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
        var recipes = new List<Recipe> { new Recipe { RecipeId = 1, Ingredients = new List<Ingredient>() } }.AsQueryable();
        var (mockContext, mockRecipes) = GetMocks();
        ConfigureDbSetMock(recipes, mockRecipes);
        var recipeOperations = new RecipeOperations(mockContext.Object);
        mockRecipes.Setup(m => m.Find(It.IsAny<object[]>())).Returns((object[] id) => recipes.FirstOrDefault(r => r.RecipeId == (int)id[0]));
        var ingredient = new Ingredient { Name = "Ingredient 1" };

        // Act
        recipeOperations.addIngredient(1, ingredient);

        // Assert
        mockRecipes.Verify(m => m.Find(It.IsAny<object[]>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
        var recipe = recipes.First();
        Assert.AreEqual(1, recipe.Ingredients.Count);
        Assert.AreEqual(ingredient.Name, recipe.Ingredients.First().Name);
    }

}
