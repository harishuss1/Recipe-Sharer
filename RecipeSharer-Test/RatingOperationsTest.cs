using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recipes;
using Users;

[TestClass]
public class RatingOperationsTests
{
    private RatingOperations _ratingOperations;
    private User _user;
    private Recipe _recipe;

    [TestInitialize]

    public void TestInitialize()
    {
        _ratingOperations = new RatingOperations();
        _user = new User("chefJohn", "securepassword123");  // Assuming User and Recipe constructors
        _recipe = new Recipe(
            _user,
            "Chocolate Cake",
            "Delicious and rich chocolate cake.",
            TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(60),
            4
        );
    }

    [TestMethod]
    public void AddRating_ValidInputs_ShouldAddRating()
    {
        // Arrange
        int initialCount = _recipe.Ratings.Count;

        // Act
        _ratingOperations.AddRating(_user, _recipe, 5);

        // Assert
        Assert.AreEqual(initialCount + 1, _recipe.Ratings.Count);
        Assert.AreEqual(5, _recipe.Ratings.Last().Score);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddRating_InvalidRating_ShouldThrowArgumentException()
    {
        // Act
        _ratingOperations.AddRating(_user, _recipe, 11); 
    }

    [TestMethod]
    public void RemoveRating_ExistingRating_ShouldRemoveRating()
    {
        // Arrange
        _ratingOperations.AddRating(_user, _recipe, 8);

        // Act
        _ratingOperations.RemoveRating(_user, _recipe);

        // Assert
        Assert.IsFalse(_recipe.Ratings.Any());
    }

    [TestMethod]
    public void UpdateRating_ExistingRating_ShouldUpdateRating()
    {
        // Arrange
        _ratingOperations.AddRating(_user, _recipe, 6);

        // Act
        _ratingOperations.UpdateRating(_user, _recipe, 9);

        // Assert
        Assert.AreEqual(9, _recipe.Ratings.First().Score);
    }

    [TestMethod]
    public void ViewRating_NoRatingAvailable_ShouldReturnZero()
    {
        // Act
        double rating = _ratingOperations.ViewRating(_recipe);

        // Assert
        Assert.AreEqual(0, rating);
    }
}