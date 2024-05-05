using Microsoft.VisualStudio.TestTools.UnitTesting;
using Context;
using Microsoft.EntityFrameworkCore;
using Recipes;
using Users;
using Moq;
using System.Linq;
[TestClass]
public class RatingOperationsTests
{
    private User _user;
    private Recipe _recipe;


    private static (Mock<RecipeSharerContext>, Mock<DbSet<Rating>>) GetMocks()
    {
        var mockContext = new Mock<RecipeSharerContext>();
        var mockRatings = new Mock<DbSet<Rating>>();
        mockContext.Setup(mock => mock.Ratings).Returns(mockRatings.Object);

        return (mockContext, mockRatings);
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
    public void AddRating_ValidInputs_ShouldAddRating()
    {
        // Arrange
        var ratings = new List<Rating>().AsQueryable();
        var (mockContext, mockRatings) = GetMocks();
        ConfigureDbSetMock(ratings, mockRatings);
        var ratingOperations = new RatingOperations(mockContext.Object);

        // Act
        ratingOperations.AddRating(_user, _recipe, 5);

        // Assert
        mockRatings.Verify(m => m.Add(It.IsAny<Rating>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddRating_InvalidRating_ShouldThrowArgumentException()
    {
        // Arrange
        var ratings = new List<Rating>().AsQueryable();
        var (mockContext, mockRatings) = GetMocks();
        ConfigureDbSetMock(ratings, mockRatings);
        var ratingOperations = new RatingOperations(mockContext.Object);

        // Act
        ratingOperations.AddRating(_user, _recipe, 11);
    }

    [TestMethod]
    public void RemoveRating_RatingExists_ShouldRemoveRating()
    {
        // Arrange
        var ratings = new List<Rating>
    {
        new Rating { User = _user, Recipe = _recipe, Score = 5 }
    }.AsQueryable();
        var (mockContext, mockRatings) = GetMocks();
        ConfigureDbSetMock(ratings, mockRatings);
        var ratingOperations = new RatingOperations(mockContext.Object);

        // Act
        ratingOperations.RemoveRating(_user, _recipe);

        // Assert
        mockRatings.Verify(m => m.Remove(It.IsAny<Rating>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void UpdateRating_RatingExists_ShouldUpdateRating()
    {
        // Arrange
        var ratings = new List<Rating>
    {
        new Rating { User = _user, Recipe = _recipe, Score = 5 }
    }.AsQueryable();
        var (mockContext, mockRatings) = GetMocks();
        ConfigureDbSetMock(ratings, mockRatings);
        var ratingOperations = new RatingOperations(mockContext.Object);

        // Act
        ratingOperations.UpdateRating(_user, _recipe, 8);

        // Assert
        var updatedRating = ratings.First();
        Assert.AreEqual(8, updatedRating.Score);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void ViewRating_NoRatingAvailable_ShouldReturnZero()
    {
        // Arrange
        var ratings = new List<Rating>().AsQueryable();
        var (mockContext, mockRatings) = GetMocks();
        ConfigureDbSetMock(ratings, mockRatings);
        var ratingOperations = new RatingOperations(mockContext.Object);

        // Act
        double rating = ratingOperations.ViewRating(_recipe);

        // Assert
        Assert.AreEqual(0, rating);
    }

    [TestMethod]
    public void ViewRating_RatingsAvailable_ShouldReturnAverageScore()
    {
        // Arrange
        var ratings = new List<Rating>
    {
        new Rating { User = _user, Recipe = _recipe, Score = 5 },
        new Rating { User = _user, Recipe = _recipe, Score = 7 },
        new Rating { User = _user, Recipe = _recipe, Score = 9 }
    }.AsQueryable();
        var (mockContext, mockRatings) = GetMocks();
        ConfigureDbSetMock(ratings, mockRatings);
        var ratingOperations = new RatingOperations(mockContext.Object);

        // Act
        double averageScore = ratingOperations.ViewRating(_recipe);

        // Assert
        Assert.AreEqual(7, averageScore);
    }
}