namespace Recipes;
using User;

public class Rating
{
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

    public Rating(User user, int score)
    {
        User = user;
        Score = score; 
    }
}
