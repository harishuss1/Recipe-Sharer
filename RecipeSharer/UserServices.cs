using Context;
using Users;

namespace RecipeSharer;

// db interactions related to users
public class UserServices
{
    private RecipeSharerContext _context;

    private UserServices(){}

    public void setRecipeSharerContext (RecipeSharerContext context)
    {
        _context = context;
    }

    // get a list of all the users in the db 
    public List<User> GetUsers()
    {
        List<User> users;
        try
        {
            users = _context.Users.ToList<User>();
        }
        catch
        {
            throw new ArgumentException("No users found in the database");
        }
        return users;
    }
    public User GetUser(string username)
    {
        User user;
        List<User> users;
        // getting all the users from the db
        try
        {
            users = GetUsers();
        }
        catch
        {
            return null;
        }
        // getting a specific user based on what was retrieved from the db
        try
        {
            var user_result = from usr in users
                                where usr.Username == username
                                select usr;
            user = user_result.First();

        }
        catch
        {
            return null;
        }
        return user;
    }

    // adds a new user to the db
    public User AddUser(string username, string password)
    {
        if(GetUser(username) != null)
        {
            throw new ArgumentException("This user already exists");
        }
        User user = new(username, password);
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    // deletes a user when the username and password is correct
    public bool DeleteUser(string username, string password)
    {
        User user = GetUser(username);
        if(user.DeleteUser(password))
        {
            _context.Remove(user);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}