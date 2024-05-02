using Users;

[TestClass]
public class UserTests
{

    [TestMethod]
    public void ConstructorTestWithValidDataAndReturnsValidUserObject()
    {
        //arrange + act
        User owner = new User("testperson", "testpwd");

        //assert
        Assert.Equals(owner, new User("testperson", "testpwd"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException),
    "---PASSWORD WAS NOT LONG ENOUGH---")]
    public void PasswordTooShort(){
        //Arrange + act
        User owner = new User("testperson", "a");

    }

    // Tests for User Authentication
    // - Ensure that a user can authenticate with valid credentials.
    // - Verify that authentication fails with invalid credentials.

    

    // Tests for User Registration
    // - Confirm that a new user can register with valid details.
    // - Check handling of registration with already taken username.

    // Tests for Updating User Profile
    // - Test updating user's password successfully.
    // - Test updating user's profile picture and personal description.

    // Tests for Favorites Management
    // - Verify adding a recipe to user's favorites.
    // - Verify removing a recipe from user's favorites.
    
}