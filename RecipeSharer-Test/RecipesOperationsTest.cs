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
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(new User("testperson", "testpwd123"), "Chocolate Cake", "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act
        recipeOperations.AddRecipe(recipe);

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
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(null, "Chocolate Cake", "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.AddRecipe(recipe));
    }

    // need to do this for each field?
    [TestMethod]
    public void AddRecipeTestThrowsExceptionWhenRecipeNameIsMissing()
    {
        // Arrange
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(new User("testperson", "testpwd123"), null, "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.AddRecipe(recipe));
    }

    [TestMethod]
    public void RemoveRecipeTestSuccessfullyRemovesRecipeWhenOwnerRemoves()
    {
        // Arrange
        User owner = new User("testperson", "testpwd123");
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(owner, "Chocolate Cake", "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        recipeOperations.AddRecipe(recipe);

        // Act
        recipeOperations.RemoveRecipe(recipe);

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
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(new User("testperson", "testpwd123"), "Chocolate Cake", "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        recipeOperations.AddRecipe(recipe);

        // Act
        recipeOperations.RemoveRecipe(recipe);

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
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe recipe = new Recipe(new User("testperson", "testpwd123"), "Chocolate Cake", "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act
        recipeOperations.RemoveRecipe(recipe);

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
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe existingRecipe = new Recipe(new User("testperson", "testpwd123"), "Chocolate Cake", "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        Recipe newDetails = new Recipe(existingRecipe.Owner, "Vanilla Cake", "Delicious vanilla cake recipe", TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act
        recipeOperations.AddRecipe(existingRecipe);
        recipeOperations.UpdateRecipe(existingRecipe, newDetails);

        // Assert
        Assert.AreEqual(newDetails.Name, existingRecipe.Name);
        Assert.AreEqual(newDetails.ShortDescription, existingRecipe.ShortDescription);
        Assert.AreEqual(newDetails.PreparationTime, existingRecipe.PreparationTime);
        Assert.AreEqual(newDetails.CookingTime, existingRecipe.CookingTime);
        Assert.AreEqual(newDetails.TotalTime, existingRecipe.TotalTime);
        Assert.AreEqual(newDetails.Servings, existingRecipe.Servings);
        Assert.AreEqual(newDetails.Steps, existingRecipe.Steps);
        Assert.AreEqual(newDetails.Ingredients, existingRecipe.Ingredients);
        Assert.Equals(newDetails.Tags, existingRecipe.Tags);
    }

    // next 3 methods, I think I would need to call the update method...? but it will fail before it reaches the end of the test soo????
    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentExceptionWhenNewOwnerIsDifferent()
    {
        // Arrange
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe existingRecipe = new Recipe(new User("testperson", "testpwd123"), "Chocolate Cake", "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);
        Recipe newDetails = new Recipe(new User("testperson2", "testpwd123"), "Vanilla Cake", "Delicious vanilla cake recipe", TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => recipeOperations.UpdateRecipe(existingRecipe, newDetails));
    }

    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentNullExceptionWhenExistingRecipeIsNull()
    {
        // Arrange
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe newDetails = new Recipe(new User("testperson", "testpwd123"), "Vanilla Cake", "Delicious vanilla cake recipe", TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(40), 10);

        // Act and Assert
        Assert.ThrowsException<NullReferenceException>(() => recipeOperations.UpdateRecipe(null, newDetails));
    }

    [TestMethod]
    public void UpdateRecipeTestThrowsArgumentNullExceptionWhenNewDetailsIsNull()
    {
        // Arrange
        RecipeOperations recipeOperations = new RecipeOperations();
        Recipe existingRecipe = new Recipe(new User("testperson", "testpwd123"), "Chocolate Cake", "Delicious chocolate cake recipe", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45), 8);

        // Act and Assert
        Assert.ThrowsException<NullReferenceException>(() => recipeOperations.UpdateRecipe(existingRecipe, null));
    }
}
