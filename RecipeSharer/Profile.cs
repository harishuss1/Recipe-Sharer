namespace Users;
using Recipes;

using System.Linq;

public class Profile
{
    public User User { get; set; }


    //To-do: Create system for profile picture, need to figure that out.
    private string? _pfpPath;
    public string ProfilePicturePath
    {
        get
        {
            return _pfpPath ?? "DEFAULTPATH";
        }
        set
        {
            _pfpPath = value;
        }
    }

    private string? Description { get; set; }

    public Profile(User user)
    {
        User = user;
    }

    public override string ToString()
    {
        return $"Name: {User.Username}, Description: {Description}, ProfilePicturePath: {ProfilePicturePath}";
    }

}