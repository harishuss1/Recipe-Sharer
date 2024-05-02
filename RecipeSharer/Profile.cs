namespace Users;
using Recipes;

using System.Linq;

public class Profile
{
    public User User { get; set; }


    //To-do: Create system for profile picture, need to figure that out.
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

    public Profile(User user)
    {
        User = user;
    }

    public override string ToString()
    {
        return $"Name: {User.Username}, Description: {Description}, ProfilePicture: {ProfilePicture.Length} bytes";
    }

}