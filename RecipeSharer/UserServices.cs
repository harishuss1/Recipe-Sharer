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

    
}