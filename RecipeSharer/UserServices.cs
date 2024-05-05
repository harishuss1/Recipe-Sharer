using System.ComponentModel;
using System.Runtime;
using Context;
using Recipes;
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
    public User AddUser(string username, string password, byte[] profilePic, string description, List<Recipe> userFavouriteRecipes)
    {
        if(GetUser(username) != null)
        {
            throw new ArgumentException("This user already exists");
        }
        User user = new(username, password, profilePic, description, userFavouriteRecipes);
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    // changes a users password and saves the change in the db
    public void ChangePassword(User user, string oldPassword, string newPassword)
    {
        try
        {
            user.ChangePassword(oldPassword, newPassword);
            _context.SaveChanges();
            
        }
        catch
        {
            throw new ArgumentException("Password was not changed");
        }

    }

    // updates a user's info 
    public void UpdateUserProfile(User user, byte[] profilePic, string description, List<Recipe> userFavouriteRecipes)
    {
        User profile = GetUser(user.Username);
        try
        {
            profile.UpdateUserProfile(user, profilePic, description, userFavouriteRecipes);
            _context.SaveChanges();
        }
        catch
        {
            throw new ArgumentException("Profile not updated");
        }
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

    public void AddToFavorites(Recipe recipe, User user)
    {
        if(user.AddToFavorites(recipe))
        {
            // checks to see if the recipe already exists in the db
            var existingRecipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == recipe.RecipeId);
        
            if(existingRecipe == null)
            {
                _context.Recipes.Add(recipe);
            }
            else
            {
                _context.Attach(existingRecipe);
            }
        }
        user.UserFavouriteRecipes.Add(recipe);
        _context.SaveChanges();
    }

    public void RemoveRecipeFromFavorites(Recipe recipe, User user)
    {
        if(user.RemoveRecipeFromFavorites(recipe))
        {
            var existingRecipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == recipe.RecipeId);
            if(existingRecipe != null)
            {
                _context.Recipes.Remove(existingRecipe);
                _context.SaveChanges();
            }
        }
    }
}