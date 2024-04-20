namespace Search;
using Recipe;
public class Search
{
    public List<string> Ingredients { get; private set; }
    public List<string> Tags { get; private set; }
    public string Keyword { get; private set; }
    public TimeSpan? MinDuration { get; private set; }
    public TimeSpan? MaxDuration { get; private set; }
    public double? MinimumRating { get; private set; }
    public List<Recipe> UserFavorites { get; private set; }
    public int? MaxServings { get; private set; }
    public int? MinServings { get; private set; }
    public string OwnerUsername { get; private set; }

    public Search()
    {
        Ingredients = new List<string>();
        Tags = new List<string>();
        UserFavorites = new List<Recipe>();
    }

    // Methods to add search criteria

    public void SetIngredients(List<string> ingredients)
    {
        if (ingredients == null || ingredients.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Ingredients list cannot be null or contain empty strings.");
        Ingredients = ingredients;
    }

    public void AddTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            throw new ArgumentException("Tag cannot be empty or whitespace.");
        Tags.Add(tag);
    }

    public void SetKeyword(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new ArgumentException("Keyword cannot be empty or whitespace.");
        Keyword = keyword;
    }

    public void SetOwnerUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Owner username cannot be empty or whitespace.");
        OwnerUsername = username;
    }


}
