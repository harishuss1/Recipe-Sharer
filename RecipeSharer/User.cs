using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Recipes;

namespace Users;

public class User
{
// admin user? 
// maybe another class for usersOperation if we think there are too many methods in this class
// public class Users

    // username property
    public string Username;
    // 
    public byte[] Salt{get; set;}
    public byte[] Password {get; set;}

    public List<Recipe> UserRecipes;

    public List<Recipe> UserFavouriteRecipes;
    
    public void addToFavorites(Recipe recipe)
    {
        UserFavouriteRecipes.Append(recipe);
    }

    public void RemoveRecipeFromFavorites(Recipe recipe)
    {
        if (UserFavouriteRecipes.Contains(recipe))
        {
            UserFavouriteRecipes.Remove(recipe);
        }
    }
    // Constructor
    public User(string name, string password){
        if (password.Length < UserGlobalVars.PASSWORD_LENGTH || password == null) {
                throw new ArgumentOutOfRangeException("---PASSWORD WAS NOT LONG ENOUGH---");
            }
            
        Username = name;
        Tuple<byte[], byte[]> hash = CreatePassword(password);
        Salt = hash.Item1;
        Password = hash.Item2;
        UserRecipes = new List<Recipe>();
        UserFavouriteRecipes = new List<Recipe>();
    }
    
     public static Tuple<byte[], byte[]> CreatePassword(string password) {
        byte[] salt = new byte[8];
            
        // RNGCryptoServiceProvider was obsolete, using RandomNumberGenerator.Create instead

        // using (RNGCryptoServiceProvider rngCsp = new()) {
        //     rngCsp.GetBytes(salt);
        // }

        using (var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(salt);
        }

        
        int iterations = 1000;
        Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
        byte[] encrypted_password = key.GetBytes(32);

        return new Tuple<byte[], byte[]>(salt, encrypted_password);
    }

    public static bool VerifyPassword(string password, byte[] salt, byte[] encrypted_password) {
        int iterations = 1000;
        Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
        byte[] encrypted_password2 = key.GetBytes(32);

        for (int i = 0; i < encrypted_password.Length; i++) {
            if (encrypted_password[i] != encrypted_password2[i]) {
                return false;
            }
        }

        return true;
    }

    public void ChangePassword(string newPassword, string oldPassword){
        if (newPassword.Length < UserGlobalVars.PASSWORD_LENGTH) {
            throw new ArgumentOutOfRangeException("PASSWORD WAS NOT LONG ENOUGH");
        }

        if (!VerifyPassword(oldPassword, Salt, Password)){
            throw new Exception("OLD PASSWORD DOES NOT MATCH");
        }

        else {
            Tuple<byte[], byte[]> hash = CreatePassword(newPassword);
            Salt = hash.Item1;
            Password = hash.Item2;
        }
    }

    

    //// These all will probably just end up being setters for the properties
    // Add Profile picture
    // removeProfilePicture method

// Some methods might be moved to UserOperations not 100% sure.
}


