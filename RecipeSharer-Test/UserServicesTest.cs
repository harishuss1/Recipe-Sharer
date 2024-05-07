using System;
using System.Collections.Generic;
using System.Linq;
using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Recipes;
using RecipeSharer;
using Users;

namespace RecipeSharerTests;

[TestClass]
public class UserServicesTest
{
    /*[TestMethod]
    public void GetUsers_NoUsersInDatabase_ReturnsEmptyList()
    {
    // Arrange
    var mockContext = new Mock<RecipeSharerContext>();
    var mockUsers = new Mock<DbSet<User>>();
    mockContext.Setup(mock => mock.Users).Returns(mockUsers.Object);

    // Act
    var result = _userServices.GetUsers();

    // Assert
    Assert.AreEqual(0, result.Count);
    }*/

    [TestMethod]
    public void GetUserTest()
    {
        // Arrange
        var listdata = new List<User>
        {
            new User("user1", "testpassword1", null, null, null),
            new User("user2", "testpassword2", null, null, null)
        };
        var data = listdata.AsQueryable();

        var mockUsers = new Mock<DbSet<User>>();
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(data.Provider);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(data.Expression);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(data.ElementType);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(mock => mock.Users).Returns(mockUsers.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        var user = service.GetUser("user1");

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual("user1", user.Username);
    }

    [TestMethod]
    public void GetUsersTest()
    {
        // Arrange
        var listdata = new List<User>
        {
            new User("user1", "testpassword1", null, null, null),
            new User("user2", "testpassword2", null, null, null)
        };
        var data = listdata.AsQueryable();

        var mockUsers = new Mock<DbSet<User>>();
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(data.Provider);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(data.Expression);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(data.ElementType);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(mock => mock.Users).Returns(mockUsers.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        var users = service.GetUsers();

        // Assert
        Assert.AreEqual(2, users.Count);
        Assert.AreEqual("user1", users[0].Username);
        Assert.AreEqual("user2", users[1].Username);
    }

    [TestMethod]
    public void UserLoginTest()
    {
        //Arrange
        var listdata = new List<User>
        {
            new User("user1", "testpassword1", null, null, null),
            new User("user2", "testpassword12", null, null, null)
        };

        var data = listdata.AsQueryable();

        var mockUsers = new Mock<DbSet<User>>();
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(data.Provider);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(data.Expression);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(data.ElementType);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(mock => mock.Users).Returns(mockUsers.Object);

        var service = new UserServices(mockContext.Object);

        //Act
        var users = service.UserLogin("user1", "testpassword1");

        //Assert
        Assert.IsTrue(users);
    }

    [TestMethod]
    public void UserLogin_InvalidUsernameTest()
    {
        // Arrange
        var listdata = new List<User>
        {
            new User("user1", "testpassword1", null, null, null),
            new User("user2", "testpassword12", null, null, null)
        };

        var data = listdata.AsQueryable();

        var mockUsers = new Mock<DbSet<User>>();
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(data.Provider);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(data.Expression);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(data.ElementType);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(m => m.Users).Returns(mockUsers.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        var result = service.UserLogin("user3", "testpassword123");

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void UserLogin_InvalidPasswordTest()
    {
        // Arrange
        var listdata = new List<User>
        {
            new User("user1", "testpassword1", null, null, null),
            new User("user2", "testpassword12", null, null, null)
        };

        var data = listdata.AsQueryable();

        var mockUsers = new Mock<DbSet<User>>();
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(data.Provider);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(data.Expression);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(data.ElementType);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(m => m.Users).Returns(mockUsers.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        var result = service.UserLogin("user1", "testpassword2");

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void AddsUserTest()
    {
        // Arrange
        var mockUsers = new Mock<DbSet<User>>();
        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(mock => mock.Users).Returns(mockUsers.Object);
        var service = new UserServices(mockContext.Object);

        // Act
        service.AddUser("user1", "testpassword1", null, null, null);

        // Assert
        mockUsers.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
        mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ChangePasswordTest()
    {
        // Arrange
        var user = new User("user1", "oldpassword", new byte[10], "new description", new List<Recipe>());
        var mockContext = new Mock<RecipeSharerContext>();
        var service = new UserServices(mockContext.Object);

        // Act
        bool result = service.ChangePassword(user, "newpassword", "oldpassword");

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void UpdateUserProfileTest()
    {
       // Arrange
        var user = new User("user1", "testpassword1", new byte[10], "test description", new List<Recipe>());
        var mockUsers = new Mock<DbSet<User>>();
        var data = new List<User> { user }.AsQueryable();
        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        service.UpdateUserProfile(user, null, "New description", new List<Recipe>());

        // Assert
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once());
        Assert.AreEqual("New description", user.Description);
    }

    [TestMethod]
    public void RemoveUserProfileTest()
    {
        // Arrange
        var user = new User("user1", "testpassword1", new byte[10], "test description", new List<Recipe>());
        var data = new List<User> { user }.AsQueryable();
        var mockUsers = new Mock<DbSet<User>>();
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(data.Provider);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(data.Expression);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(data.ElementType);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(mock => mock.Users).Returns(mockUsers.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        service.RemoveUserProfile(user);

        // Assert
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once());
        Assert.AreEqual("", user.Description);
        Assert.IsNull(user.ProfilePicture);
        Assert.AreEqual(0, user.UserFavouriteRecipes.Count);
    }

    [TestMethod]
    public void DeleteUserTest()
    {
        // Arrange
        var user = new User("user1", "testpassword1", null, null, null);
        var data = new List<User> { user }.AsQueryable();
        var mockUsers = new Mock<DbSet<User>>();
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(data.Provider);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(data.Expression);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(data.ElementType);
        mockUsers.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(mock => mock.Users).Returns(mockUsers.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        var result = service.DeleteUser("user1", "testpassword1");

        // Assert
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once());
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void AddToFavoritesTest()
    {
        // Arrange
        var user = new User("user1", "testpassword1", new byte[10], "test description", new List<Recipe>());
        var recipe = new Recipe();
        var dataUsers = new List<User> { user }.AsQueryable();
        var dataRecipes = new List<Recipe> { recipe }.AsQueryable();
        var mockSetUsers = new Mock<DbSet<User>>();
        var mockSetRecipes = new Mock<DbSet<Recipe>>();
        mockSetUsers.As<IQueryable<User>>().Setup(m => m.Provider).Returns(dataUsers.Provider);
        mockSetUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(dataUsers.Expression);
        mockSetUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(dataUsers.ElementType);
        mockSetUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(dataUsers.GetEnumerator());
        mockSetRecipes.As<IQueryable<Recipe>>().Setup(m => m.Provider).Returns(dataRecipes.Provider);
        mockSetRecipes.As<IQueryable<Recipe>>().Setup(m => m.Expression).Returns(dataRecipes.Expression);
        mockSetRecipes.As<IQueryable<Recipe>>().Setup(m => m.ElementType).Returns(dataRecipes.ElementType);
        mockSetRecipes.As<IQueryable<Recipe>>().Setup(m => m.GetEnumerator()).Returns(dataRecipes.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(c => c.Users).Returns(mockSetUsers.Object);
        mockContext.Setup(c => c.Recipes).Returns(mockSetRecipes.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        service.AddToFavorites(recipe, user);

        // Assert
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once());
        Assert.IsTrue(user.UserFavouriteRecipes.Contains(recipe));
    }

    [TestMethod]
    public void RemoveRecipeFromFavorites_RemovesFromFavorites()
    {
        // Arrange
        var user = new User("user1", "testpassword1", new byte[10], "test description", new List<Recipe>());
        var recipe = new Recipe();
        user.AddToFavorites(recipe);
        var dataUsers = new List<User> { user }.AsQueryable();
        var dataRecipes = new List<Recipe> { recipe }.AsQueryable();
        var mockSetUsers = new Mock<DbSet<User>>();
        var mockSetRecipes = new Mock<DbSet<Recipe>>();
        mockSetUsers.As<IQueryable<User>>().Setup(m => m.Provider).Returns(dataUsers.Provider);
        mockSetUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(dataUsers.Expression);
        mockSetUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(dataUsers.ElementType);
        mockSetUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(dataUsers.GetEnumerator());
        mockSetRecipes.As<IQueryable<Recipe>>().Setup(m => m.Provider).Returns(dataRecipes.Provider);
        mockSetRecipes.As<IQueryable<Recipe>>().Setup(m => m.Expression).Returns(dataRecipes.Expression);
        mockSetRecipes.As<IQueryable<Recipe>>().Setup(m => m.ElementType).Returns(dataRecipes.ElementType);
        mockSetRecipes.As<IQueryable<Recipe>>().Setup(m => m.GetEnumerator()).Returns(dataRecipes.GetEnumerator());

        var mockContext = new Mock<RecipeSharerContext>();
        mockContext.Setup(c => c.Users).Returns(mockSetUsers.Object);
        mockContext.Setup(c => c.Recipes).Returns(mockSetRecipes.Object);

        var service = new UserServices(mockContext.Object);

        // Act
        service.RemoveRecipeFromFavorites(recipe, user);

        // Assert
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once());
        Assert.IsFalse(user.UserFavouriteRecipes.Contains(recipe));
    }
}