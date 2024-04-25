namespace RecipeSharerTests;
using Recipes;
using System;
using Users;
using System.Collections.Generic;
using System.Linq;
// Tests for Adding Recipes
// - Ensure that a recipe can be added with valid details.
// - Check handling of adding a recipe with missing or invalid fields.

// Tests for Updating Recipes
// - Verify that a recipe can be updated by its owner.
// - Test updating a recipe with invalid changes (e.g., empty name or description).

// Tests for Removing Recipes
// - Ensure that a recipe can be removed by its owner.
// - Test attempt to remove a recipe by a non-owner.

// Tests for Rating Recipes -> should be in another file (RatingTest.cs)
// - Verify that a recipe can be rated.
// - Check that rating updates affect the overall recipe rating correctly.

[TestClass]
public class RecipeOperationsTests
{
    // Test for AddRecipe method
    [TestMethod]
    public void AddRecipeTestAddsValidRecipe()
    {
        // Arrange
        User _user = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(_user, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act
        recipeOperations.AddRecipe(_user,recipe);

        // Assert
        // iterates through each item in the recipes list and if it finds an item that equals the recipe object using the Equals() method, it sets recipeFound to true. 
        bool recipeFound = false;
        foreach (var item in recipeOperations.recipes)
        {
            if (item.Equals(recipe))
            {
                recipeFound = true;
                break;
            }
        }
        Assert.IsTrue(recipeFound);
    }

    [TestMethod]
    public void AddRecipeTestThrowsArgumentExceptionWhenRecipeHasNullOwner()
    {
        // Arrange
        User _user = new User(null, "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(_user, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.AddRecipe(_user,recipe));
    }

    // need to do this for each field?
    [TestMethod]
    public void AddRecipeTestThrowsExceptionWhenRecipeNameIsMissing()
    {
        // Arrange
        User _user = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(_user, null, "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.AddRecipe(_user,recipe));
    }

    [TestMethod]
    public void RemoveRecipeTestSuccessfullyRemovesRecipeWhenOwnerRemoves()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        recipeOperations.AddRecipe(owner,recipe);

        // Act
        recipeOperations.RemoveRecipe(owner,recipe);

        // Assert
        bool recipeFound = false;
        foreach (var item in recipeOperations.recipes)
        {
            if (item.Equals(recipe))
            {
                recipeFound = true;
                break;
            }
        }
        Assert.IsFalse(recipeFound);
    }

    // Test for RemoveRecipe method
    [TestMethod]
    public void RemoveRecipeTestRemovesExistingRecipe()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        recipeOperations.AddRecipe(owner,recipe);

        // Act
        recipeOperations.RemoveRecipe(owner,recipe);

        // Assert
        bool recipeNotFound = true;
        foreach (var item in recipeOperations.recipes)
        {
            if (item.Equals(recipe))
            {
                recipeNotFound = false;
                break;
            }
        }
        Assert.IsTrue(recipeNotFound);
    }

    [TestMethod]
    public void RemoveRecipeTestDoesNothingWhenRecipeDoesNotExist()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act
        recipeOperations.RemoveRecipe(owner,recipe);

        // Assert
        bool recipeNotFound = true;
        foreach (var item in recipeOperations.recipes)
        {
            if (item.Equals(recipe))
            {
                recipeNotFound = false;
                break;
            }
        }
        Assert.IsTrue(recipeNotFound);
    }

    // Test for UpdateRecipe method
    [TestMethod]
    public void UpdateRecipeTestUpdatesExistingRecipeWithValidDetails()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe existingRecipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        Recipe newDetails = new Recipe(existingRecipe.Owner, "Vanilla Cake", "Delicious vanilla cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act
        recipeOperations.AddRecipe(owner,existingRecipe);
        recipeOperations.UpdateRecipe(owner,existingRecipe, newDetails);

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
    }

    // next 3 methods, I think I would need to call the update method...? but it will fail before it reaches the end of the test soo????
    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentExceptionWhenNewOwnerIsDifferent()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        User owner2 = new User("testperson2", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe existingRecipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        Recipe newDetails = new Recipe(owner2, "Vanilla Cake", "Delicious vanilla cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.UpdateRecipe(owner,existingRecipe, newDetails));
    }

    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentNullExceptionWhenExistingRecipeIsNull()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe newDetails = new Recipe(owner, "Vanilla Cake", "Delicious vanilla cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act and Assert
        Assert.ThrowsException<NullReferenceException>(() => recipeOperations.UpdateRecipe(owner,null, newDetails));
    }

    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentNullExceptionWhenNewDetailsIsNull()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe existingRecipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", new List<Ingredient>(), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act and Assert
        Assert.ThrowsException<NullReferenceException>(() => recipeOperations.UpdateRecipe(owner,existingRecipe, null));
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsSteps()
    {
        // Arrange
        RecipeOperations recipeOperations = new RecipeOperations();
        var input = "Step 1\nStep 2\nDone!";
        var reader = new StringReader(input);
        var expectedSteps = new List<string> { "Step 1", "Step 2" };

        // Act
        var steps = recipeOperations.AddStepsToRecipe(reader);

        // Assert
        CollectionAssert.AreEqual(expectedSteps, steps);
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsEmptyListWhenNoStepsEntered()
    {
        // Arrange
        RecipeOperations recipeOperations = new RecipeOperations();
        var input = "Done!";
        var reader = new StringReader(input);

        // Act
        var steps = recipeOperations.AddStepsToRecipe(reader);

        // Assert
        CollectionAssert.AreEqual(new List<string>(), steps);
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsStepsWhenSingleStepEntered()
    {
        // Arrange
        RecipeOperations recipeOperations = new RecipeOperations();
        var input = "Step 1\nDone!";
        var reader = new StringReader(input);
        var expectedSteps = new List<string> { "Step 1" };

        // Act
        var steps = recipeOperations.AddStepsToRecipe(reader);

        // Assert
        CollectionAssert.AreEqual(expectedSteps, steps);
    }

    [TestMethod]
    public void AddStepsToRecipeReturnsStepsIgnoresLeadingAndTrailingSpaces()
    {
        // Arrange
        var recipeOperations = new RecipeOperations();
        var input = "  Step 1  \n  Step 2  \nDone!";
        var reader = new StringReader(input);
        var expectedSteps = new List<string> { "Step 1", "Step 2" };

        // Act
        var steps = recipeOperations.AddStepsToRecipe(reader);

        // Assert
        CollectionAssert.AreEqual(expectedSteps, steps);
    }
}
