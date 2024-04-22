namespace users;

using System.Linq;

public class Profile {
    public User User {get; set;}
    
    private string? _pfpPath;

    //To-do: Create system for profile picture, need to figure that out.
    public string ProfilePicturePath {
        get{
            return _pfpPath ?? "DEFAULTPATH";
        } 
        set{
            _pfpPath = value;
        }
    }

    private string? Description {get; set;}

    public List<Recipe> Favorites {get; set;} = new List<Recipe>();

    public Profile(User user){
        User = user;
    }

    public void addToFavorites(Recipe recipe){
        Favorites.Append(recipe);
    }

    public override string ToString(){
        return $"Name: {User.Username}, Description: {Description}, ProfilePicturePath: {ProfilePicturePath}";
    }
}