namespace Recipes;
using Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Rating
{
    [Key]
    public int RatingId {get; set;}

    [ForeignKey("RecipeId")]
    public Recipe Recipe {get;set;}
    [ForeignKey("UserId")]
    public User User { get; set; }
    private int _score;

    public int Score
    {
        get { return _score; }
        set
        {
            if (value < 0 || value > 10)
            {
                throw new ArgumentOutOfRangeException("Score must be between 0 and 10.");
            }
            _score = value;
        }
    }
    public Rating(){
    }
}


// var rating = new Rating(a, b);

// var rating = new Rating
// {
// a = a,
// b = b,
// };

// var rating = new Rating();
// rating.a = a;