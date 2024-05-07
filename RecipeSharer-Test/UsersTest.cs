using Users;
using Moq;
using Recipes;
namespace RecipeSharerTests;
[TestClass]
public class UserTestMethods
{
    [TestMethod]
    public void CreateUser_ValidInput_ReturnsUser()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();

        // Act
        User user = new User(username, password, profilePicture, description, favoriteRecipes);

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(username, user.Username);
        Assert.AreEqual(profilePicture, user.ProfilePicture);
        Assert.AreEqual(description, user.Description);
        CollectionAssert.AreEqual(favoriteRecipes, user.UserFavouriteRecipes);
    }

    [TestMethod]
    public void CreateUser_EmptyUsername_ThrowsArgumentException()
    {
        // Arrange
        string username = "";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => new User(username, password, profilePicture, description, favoriteRecipes));
    }

    [TestMethod]
    public void CreateUser_ShortPassword_ThrowsArgumentException()
    {
        // Arrange
        string username = "testuser";
        string password = "pwd"; 
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => new User(username, password, profilePicture, description, favoriteRecipes));
    }

    [TestMethod]
    public void ChangePassword_ValidInput_PasswordChanged()
    {
        // Arrange
        string username = "testuser";
        string oldPassword = "oldpasswd";
        string newPassword = "newpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, oldPassword, profilePicture, description, favoriteRecipes);

        // Act
        user.ChangePassword(newPassword, oldPassword);

        // Assert
        Assert.IsTrue(User.VerifyPassword(newPassword, user.Salt, user.Password));
    }

    [TestMethod]
    public void ChangePassword_ShortNewPassword_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        string username = "testuser";
        string oldPassword = "oldpasswd";
        string newPassword = "pwd"; 
        byte[] profilePicture = new byte[10];
        string description = "TestMethod description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, oldPassword, profilePicture, description, favoriteRecipes);

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => user.ChangePassword(newPassword, oldPassword));
    }

    [TestMethod]
    public void ChangePassword_IncorrectOldPassword_ThrowsException()
    {
        // Arrange
        string username = "testuser";
        string oldPassword = "oldpasswd";
        string newPassword = "newpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, oldPassword, profilePicture, description, favoriteRecipes);

        // Act & Assert
        Assert.ThrowsException<Exception>(() => user.ChangePassword(newPassword, "incorrectoldpassword"));
    }

    [TestMethod]
    public void UserLogin_CorrectCredentials_ReturnsTrue()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);

        // Act & Assert
        Assert.IsTrue(user.UserLogin(username, password));
    }

    [TestMethod]
    public void UserLogin_IncorrectUsername_ReturnsFalse()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);

        // Act & Assert
        Assert.IsFalse(user.UserLogin("incorrectusername", password));
    }

    [TestMethod]
    public void UserLogin_IncorrectPassword_ReturnsFalse()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);

        // Act & Assert
        Assert.IsFalse(user.UserLogin(username, "incorrectpassword"));
    }

    [TestMethod]
    public void DeleteUser_CorrectPassword_ReturnsTrue()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);

        // Act & Assert
        Assert.IsTrue(user.DeleteUser(password));
    }

    [TestMethod]
    public void DeleteUser_IncorrectPassword_ReturnsFalse()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);

        // Act & Assert
        Assert.IsFalse(user.DeleteUser("incorrectpassword"));
    }

    [TestMethod]
    public void AddToFavorites_NewRecipe_ReturnsTrue()
    {
        // Arrange
        Recipe recipe = new Recipe();
        User user = new User();

        // Act
        bool result = user.AddToFavorites(recipe);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.Contains(user.UserFavouriteRecipes, recipe);
    }

    [TestMethod]
    public void AddToFavorites_ExistingRecipe_ReturnsFalse()
    {
        // Arrange
        Recipe recipe = new Recipe();
        User user = new User();
        user.UserFavouriteRecipes = new List<Recipe> { recipe };

        // Act
        bool result = user.AddToFavorites(recipe);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void RemoveRecipeFromFavorites_ExistingRecipe_ReturnsTrue()
    {
        // Arrange
        Recipe recipe = new Recipe();
        User user = new User();
        user.UserFavouriteRecipes = new List<Recipe> { recipe };

        // Act
        bool result = user.RemoveRecipeFromFavorites(recipe);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.DoesNotContain(user.UserFavouriteRecipes, recipe);
    }

    [TestMethod]
    public void RemoveRecipeFromFavorites_NonExistingRecipe_ReturnsFalse()
    {
        // Arrange
        Recipe recipe = new Recipe();
        User user = new User();

        // Act
        bool result = user.RemoveRecipeFromFavorites(recipe);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void UpdateUserProfile_ValidInput_ProfileUpdated()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);
        byte[] newProfilePicture = new byte[20];
        string newDescription = "New description";
        List<Recipe> newFavoriteRecipes = new List<Recipe> { new Recipe() };

        // Act
        user.UpdateUserProfile(user, newProfilePicture, newDescription, newFavoriteRecipes);

        // Assert
        CollectionAssert.AreEqual(newProfilePicture, user.ProfilePicture);
        Assert.AreEqual(newDescription, user.Description);
        CollectionAssert.AreEqual(newFavoriteRecipes, user.UserFavouriteRecipes);
    }

    [TestMethod]
    public void UpdateUserProfile_InvalidProfilePic_ProfileNotUpdated()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);
        byte[] newProfilePicture = null;
        string newDescription = "New description";
        List<Recipe> newFavoriteRecipes = new List<Recipe>();

        // Act
        bool result = user.UpdateUserProfile(user, newProfilePicture, newDescription, newFavoriteRecipes);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void UpdateUserProfile_InvalidDescription_ProfileNotUpdated()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);
        byte[] newProfilePicture = null;
        string newDescription = "New description";
        List<Recipe> newFavoriteRecipes = new List<Recipe>();

        // Act
        bool result = user.UpdateUserProfile(user, newProfilePicture, newDescription, newFavoriteRecipes);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void UpdateUserProfile_EmptyFavoriteRecipes_ProfileNotUpdated()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);
        byte[] newProfilePicture = new byte[20];
        string newDescription = "New description";
        List<Recipe> newFavoriteRecipes = new List<Recipe>(); // Empty favorite recipes

        // Act
        bool result = user.UpdateUserProfile(user, newProfilePicture, newDescription, newFavoriteRecipes);

        // Assert
        Assert.IsFalse(result);
    }


    [TestMethod]
    public void RemoveUserProfile_ProfileRemoved()
    {
        // Arrange
        string username = "testuser";
        string password = "testpasswd";
        byte[] profilePicture = new byte[10];
        string description = "Test description";
        List<Recipe> favoriteRecipes = new List<Recipe>();
        User user = new User(username, password, profilePicture, description, favoriteRecipes);

        // Act
        user.RemoveUserProfile();

        // Assert
        Assert.IsNull(user.ProfilePicture);
        Assert.AreEqual("", user.Description);
        Assert.AreEqual(0, user.UserFavouriteRecipes.Count);
    }

    [TestMethod]
    public void VerifyProfilePic_NullPic_ReturnsFalse()
    {
        // Arrange & Act & Assert
        Assert.IsFalse(User.VerifyProfilePic(null));
    }

    [TestMethod]
    public void VerifyProfilePic_EmptyPic_ReturnsFalse()
    {
        // Arrange & Act & Assert
        Assert.IsFalse(User.VerifyProfilePic(new byte[0]));
    }

    [TestMethod]
    public void VerifyProfilePic_NonEmptyPic_ReturnsTrue()
    {
        // Arrange & Act & Assert
        Assert.IsTrue(User.VerifyProfilePic(new byte[1]));
    }

    [TestMethod]
    public void VerifyDescription_NullDescription_ReturnsFalse()
    {
        // Arrange & Act & Assert
        Assert.IsFalse(User.VerifyDescription(null));
    }

    [TestMethod]
    public void VerifyDescription_EmptyDescription_ReturnsFalse()
    {
        // Arrange & Act & Assert
        Assert.IsFalse(User.VerifyDescription(""));
    }

    [TestMethod]
    public void VerifyDescription_NonEmptyDescription_ReturnsTrue()
    {
        // Arrange & Act & Assert
        Assert.IsTrue(User.VerifyDescription("TestMethod description"));
    }

    [TestMethod]
    public void VerifyUserFavoriteRecipes_EmptyList_ReturnsFalse()
    {
        // Arrange & Act & Assert
        Assert.IsFalse(User.VerifyUserFavoriteRecipes(new List<Recipe>()));
    }

    [TestMethod]
    public void VerifyUserFavoriteRecipes_NonEmptyList_ReturnsTrue()
    {
        // Arrange & Act & Assert
        Assert.IsTrue(User.VerifyUserFavoriteRecipes(new List<Recipe> { new Recipe() }));
    }

    // Tests for User Authentication
    // - Ensure that a user can authenticate with valid credentials.
    // - Verify that authentication fails with invalid credentials.

//     // Tests for User Registration
//     // - Confirm that a new user can register with valid details.
//     // - Check handling of registration with already taken username.

//     // Tests for Updating User Profile
//     // - Test updating user's password successfully.
//     // - Test updating user's profile picture and personal description.

//     // Tests for Favorites Management
//     // - Verify adding a recipe to user's favorites.
//     // - Verify removing a recipe from user's favorites.
    
}