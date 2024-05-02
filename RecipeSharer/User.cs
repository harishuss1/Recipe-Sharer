using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Recipes;

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
    public List<Recipe> UserRecipes;

    //[InverseProperty("FavoritedBy")]
    public List<Recipe> UserFavouriteRecipes;
    private byte[] _profilePicture;
    public byte[] ProfilePicture
    {
        get
        {
            return _profilePicture ?? new byte[0]; // return an empty byte array if _profilePicture is null
        }
        set
        {
            _profilePicture = value;
        }
    }

    private string? Description { get; set; }
    
    public void AddToFavorites(Recipe recipe)
    {
        if (!UserFavouriteRecipes.Contains(recipe))
        {
            UserFavouriteRecipes.Add(recipe);
        }
    }

    public void RemoveRecipeFromFavorites(Recipe recipe)
    {
        if (UserFavouriteRecipes.Contains(recipe))
        {
            UserFavouriteRecipes.Remove(recipe);
        }
    }

    public User(string username, string password)
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
    }

    public User() { }


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

    public void AddUser(User user)
    {
        // Add user to the database
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


