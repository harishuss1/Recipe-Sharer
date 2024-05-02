namespace Users;

public class UserOperation
{
    private List<User> _users;

    public UserOperation()
    {
        _users = new List<User>();
    }

    public User Register(string username, string password)
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

        var user = new User(username, password);
        _users.Add(user);

        return user;
    }

    // Other methods...
}