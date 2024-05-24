using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Recipes;
using System.Linq;

namespace Users;

public class User
{
    public const int PASSWORD_LENGTH = 8;

    // username property
    [Key]
    public int UserId{get;set;}
    public string Username{get; set;}
    // 
    public byte[] Salt{get;set;}
    public byte[] Password {get;set;}

    [InverseProperty("Owner")]
    public List<Recipe> UserRecipes{get; set;}

    [InverseProperty("FavoritedBy")]
    public List<Recipe> UserFavouriteRecipes {get; set;} = new();

    private byte[]? _profilePicture;
    public byte[]? ProfilePicture
    {
        get
        {
            return _profilePicture; // return an empty byte array if _profilePicture is null
        }
        set
        {
            _profilePicture = value;
        }
    }

    private string? _description;

    public string? Description {
        get
        {
            return _description;
        }
        set
        {
            _description = value;
        } 
    }
    
    public bool AddToFavorites(Recipe recipe)
    {
        if (UserFavouriteRecipes == null)
        {
            UserFavouriteRecipes = new List<Recipe>();
        }
        
        if (!UserFavouriteRecipes.Contains(recipe))
        {
            UserFavouriteRecipes.Add(recipe);
            return true;
        }
        return false;
    }

    public bool RemoveRecipeFromFavorites(Recipe recipe)
    {
        if (UserFavouriteRecipes == null)
        {
            return false;
        }

        if (UserFavouriteRecipes.Contains(recipe))
        {
            UserFavouriteRecipes.Remove(recipe);
            return true;
        }
        return false;
    }

    public User(string username, string password, byte[] profilePicture, string description, List<Recipe> userFavouriteRecipes)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be empty.");
        }

        if (string.IsNullOrEmpty(password) || password.Length < PASSWORD_LENGTH)
        {
            throw new ArgumentException("Password must be at least " + PASSWORD_LENGTH + " characters long.");
        }

        Username = username;
        Tuple<byte[], byte[]> hash = CreatePassword(password);
        Salt = hash.Item1;
        Password = hash.Item2;
        ProfilePicture = profilePicture;
        Description = description;
        UserFavouriteRecipes = userFavouriteRecipes;
    }

    public User(string username, string password){
         if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be empty.");
        }

        if (string.IsNullOrEmpty(password) || password.Length < PASSWORD_LENGTH)
        {
            throw new ArgumentException("Password must be at least " + PASSWORD_LENGTH + " characters long.");
        }
        Username = username;
        Tuple<byte[], byte[]> hash = CreatePassword(password);
        Salt = hash.Item1;
        Password = hash.Item2;
        ProfilePicture = null;
        Description = null;
        UserFavouriteRecipes = new List<Recipe>();
    }

    public User() { }

    public User CreateUser(string username, string password, byte[] profilePicture, string description, List<Recipe> userFavouriteRecipes) 
    {
        User user = new User(username, password, profilePicture, description, userFavouriteRecipes);
        return user;
    }

    // creating a password
    public static Tuple<byte[], byte[]> CreatePassword(string password)
    {
        byte[] salt = new byte[8];

        // RNGCryptoServiceProvider was obsolete, using RandomNumberGenerator.Create instead

        // using (RNGCryptoServiceProvider rngCsp = new()) {
        //     rngCsp.GetBytes(salt);
        // }

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }


        int iterations = 1000;
        Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
        byte[] encrypted_password = key.GetBytes(32);

        return new Tuple<byte[], byte[]>(salt, encrypted_password);
    }

    // verifies that the password is correct 
    public static bool VerifyPassword(string password, byte[] salt, byte[] encrypted_password)
    {
        int iterations = 1000;
        Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
        byte[] encrypted_password2 = key.GetBytes(32);

        for (int i = 0; i < encrypted_password.Length; i++)
        {
            if (encrypted_password[i] != encrypted_password2[i])
            {
                return false;
            }
        }

        return true;
    }

    // change password method
    public void ChangePassword(string newPassword, string oldPassword)
    {
        if (newPassword.Length < PASSWORD_LENGTH)
        {
            throw new ArgumentOutOfRangeException("PASSWORD WAS NOT LONG ENOUGH");
        }

        if (!VerifyPassword(oldPassword, Salt, Password)){
            throw new Exception("OLD PASSWORD DOES NOT MATCH");
        }

        else
        {
            Tuple<byte[], byte[]> hash = CreatePassword(newPassword);
            Salt = hash.Item1;
            Password = hash.Item2;
        }
    }

    // verifies that the username and password exists
    public bool UserLogin(string username, string password)
    {
        if(!(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)))
        {
            if(username==Username && VerifyPassword(password, Salt, Password))
            {
                return true;
            }
        }
        return false;
    }

    // makes user enter their password to confirm deleting
    public bool DeleteUser(string password)
    {
        if(UserLogin(Username, password))
        {
            return true;
        }
        return false;
    }


    public bool UpdateUserProfile(User user, byte[] profilePic, string description, List<Recipe> userFavouriteRecipes)
    {
        if(user.Username == Username)
        {
            if  (VerifyProfilePic(profilePic) && VerifyDescription(description) && VerifyUserFavoriteRecipes(userFavouriteRecipes))
            {
                ProfilePicture = profilePic;
                Description = description;
                UserFavouriteRecipes = userFavouriteRecipes;
                return true;
            }
        }
        return false;
    }

    public void RemoveUserProfile()
    {
        ProfilePicture = null;
        Description = "";
        UserFavouriteRecipes.Clear();
    }

    public static bool VerifyProfilePic(byte[] pic)
    {
        return pic != null && pic.Length > 0;
    }

    public static bool VerifyDescription(string description)
    {
        return !string.IsNullOrEmpty(description);
    }

    public static bool VerifyUserFavoriteRecipes(List<Recipe> userFavouriteRecipes)
    {
        return userFavouriteRecipes != null && userFavouriteRecipes.Count > 0;
    }


    //// These all will probably just end up being setters for the properties
    // Add Profile picture
    // removeProfilePicture method

    // Some methods might be moved to UserOperations not 100% sure.
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        User other = (User)obj;
        return UserId == other.UserId;
    }
}