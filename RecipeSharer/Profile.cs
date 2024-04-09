namespace users;

using System.Linq;
using recipes;

public class Profile {
    public User User {get; set;}
    
    private string? ProfilePicturePath {get; set;}

    private string? Description {get; set;}

    public List<Recipe> Favorites {get; set;} = new List<Recipe>();






    public void addToFavorites(Recipe recipe){
        Favorites.Append(recipe);
    }
}