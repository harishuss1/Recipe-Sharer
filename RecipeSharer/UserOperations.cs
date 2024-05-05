using Recipes;

namespace Users;

public class UserOperation
{
    private List<User> _users;

    public UserOperation()
    {
        _users = new List<User>();
    }

    public User Register(string username, string password, byte[] profilePic, string description, List<Recipe> userFavouriteRecipes)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be empty.");
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }

        if (_users.Any(u => u.Username == username))
        {
            throw new ArgumentException("Username already exists.");
        }

        var user = new User(username, password, profilePic, description, userFavouriteRecipes);
        _users.Add(user);

        return user;
    }

    // Other methods...
}