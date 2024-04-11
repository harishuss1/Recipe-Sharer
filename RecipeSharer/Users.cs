using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;


namespace users;

public class User
{
// admin user? 
// maybe another class for usersOperation if we think there are too many methods in this class
// public class Users

    // username property ? or name property
    public string Username;

    private string _password;
    public byte[] Salt{get; set;}
    public byte[] Password {get; set;}

    // Constructor
    public User(string name, string password){
        if (password.Length < UserGlobalVars.PASSWORD_LENGTH) {
                throw new ArgumentOutOfRangeException("---PASSWORD WAS NOT LONG ENOUGH---");
            }
            
        Username = name;
        Tuple<byte[], byte[]> hash = CreatePassword(password);
        Salt = hash.Item1;
        Password = hash.Item2;
    }
    
     public static Tuple<byte[], byte[]> CreatePassword(string password) {
        byte[] salt = new byte[8];
            
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
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


    // updatePassword method

    //// These all will probably just end up being setters for the properties
    // Add Profile picture
    // removeProfilePicture method

// Some methods might be moved to UserOperations not 100% sure.
}


