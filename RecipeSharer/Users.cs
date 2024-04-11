namespace users;

public class User
{
// admin user? 
// maybe another class for usersOperation if we think there are too many methods in this class
// public class Users

    // username property ? or name property
    public string Username;

    private string _password;
    public string Password {
        get {
            return _password;
        } 
        set {
            if (_password != null){
                Console.WriteLine("Enter old password");
                string passwordCheck = Console.ReadLine();
                if (passwordCheck.Equals(_password)){
                    _password = value;
                }
            }
            else {
                _password = value;
            }
        }
    }

    // Constructor
    public User(string name, string password){
        Username = name;
        Password = password;
    }

    // updatePassword method

    //// These all will probably just end up being setters for the properties
    // Add Profile picture
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


