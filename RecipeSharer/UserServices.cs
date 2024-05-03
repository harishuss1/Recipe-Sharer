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
    
}