using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Recipes;

namespace Users;

public class User
{
    public const int PASSWORD_LENGTH = 8;

    [Key]
    public int Id { get; set; }
    [Required]
    public string Username;
    private byte[] _salt;
    private byte[] _password;
    public string Password
    {
        set
        {
            if (value.Length < PASSWORD_LENGTH || value == null)
            {
                throw new ArgumentOutOfRangeException("PASSWORD WAS NOT LONG ENOUGH");
            }

            Tuple<byte[], byte[]> hash = CreatePassword(value);
            _salt = hash.Item1;
            _password = hash.Item2;

        }
    }

    public List<Recipe> UserRecipes { get; set; }

    public List<Recipe> UserFavouriteRecipes { get; set; }

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
        Password = password;
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

        if (!VerifyPassword(oldPassword, _salt, _password))
        {
            throw new Exception("OLD PASSWORD DOES NOT MATCH");
        }

        else
        {
            Tuple<byte[], byte[]> hash = CreatePassword(newPassword);
            _salt = hash.Item1;
            _password = hash.Item2;
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
        return Id == other.Id;
    }
}


