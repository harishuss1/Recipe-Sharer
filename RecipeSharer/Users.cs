namespace User;
public class User
{
// admin user? 
// maybe another class for usersOperation if we think there are too many methods in this class
// public class Users

    // username property ? or name property
    public string Username;

    private string Password;

    private string? ProfilePicturePath;

    private string? Description;

    // Constructor
    public User(string name, string password);

    // updatePassword method
    public void updatePassword(string newPassword);

    //// These all will probably just end up being setters for the properties
    // Add Profile picture
    public void addProfilePicture(string path);
    // removeProfilePicture method
    public void removeProfilePicture();
    // update pfp
    public void changeProfilePicture(string path);
    // add description
    public void addDescription(string description);
    // update description
    public void updateDescription(string description);

// Some methods might be moved to UserOperations not 100% sure.
}


