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

    public void SetTimeConstraints(TimeSpan? minDuration, TimeSpan? maxDuration)
    {
        if (minDuration.HasValue && minDuration.Value.TotalMinutes < 0)
            throw new ArgumentOutOfRangeException(nameof(minDuration), "Minimum duration cannot be less than zero.");
        if (maxDuration.HasValue && maxDuration.Value.TotalMinutes < 0)
            throw new ArgumentOutOfRangeException(nameof(maxDuration), "Maximum duration cannot be less than zero.");
        if (minDuration.HasValue && maxDuration.HasValue && minDuration > maxDuration)
            throw new ArgumentException("Minimum duration cannot be greater than maximum duration.");

        MinDuration = minDuration;
        MaxDuration = maxDuration;
    }


    public void SetServingsConstraints(int? minServings, int? maxServings)
    {
        if (minServings.HasValue && minServings < 1)
            throw new ArgumentOutOfRangeException(nameof(minServings), "Minimum servings must be at least 1.");
        if (maxServings.HasValue && maxServings < 1)
            throw new ArgumentOutOfRangeException(nameof(maxServings), "Maximum servings must be at least 1.");
        if (minServings.HasValue && maxServings.HasValue && minServings > maxServings)
            throw new ArgumentException("Minimum servings cannot be greater than maximum servings.");

        MinServings = minServings;
        MaxServings = maxServings;
    }

    public void SetMinimumRating(double? rating)
    {
        if (rating.HasValue && (rating < 0 || rating > 10))
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0 and 10.");
        MinimumRating = rating;
    }
    // The method to perform the actual search
    public List<Recipe> PerformSearch(List<Recipe> allRecipes)
    {
        IEnumerable<Recipe> query = allRecipes;

        if (Ingredients.Any())
        {
            query = query.Where(r => Ingredients.All(ing => r.Ingredients.Select(i => i.Name).Contains(ing)));
        }
        if (Tags.Any())
        {
            query = query.Where(r => Tags.All(tag => r.Tags.Contains(tag)));
        }
        if (!string.IsNullOrEmpty(Keyword))
        {
            query = query.Where(r => r.Name.Contains(Keyword, StringComparison.OrdinalIgnoreCase) ||
                                     (r.ShortDescription != null && r.ShortDescription.Contains(Keyword, StringComparison.OrdinalIgnoreCase)));
        }
        if (MinDuration.HasValue)
        {
            query = query.Where(r => r.TotalTime >= MinDuration.Value);
        }
        if (MaxDuration.HasValue)
        {
            query = query.Where(r => r.TotalTime <= MaxDuration.Value);
        }
        
        return query.ToList();
    }
}
