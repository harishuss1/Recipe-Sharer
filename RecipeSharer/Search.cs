public class Search
{
    // The search parameters properties
    public List<string> Tags { get; private set; }
    public double? MinimumRating { get; private set; }
    public string Name { get; private set; }
    public List<string> Ingredients { get; private set; }

    public string Keyword { get; private set; }

    // Constructor to initialize the search parameters
    public Search()
    {
        Tags = new List<string>();
        Ingredients = new List<string>();
    }

    // Methods to add search criteria
    public void AddTag(string tag)
    {
        if (!Tags.Contains(tag))
        {
            Tags.Add(tag);
        }
    }

    public void AddIngredient(string ingredient)
    {
        if (!Ingredients.Contains(ingredient))
        {
            Ingredients.Add(ingredient);
        }
    }

    public void SetMinimumRating(double rating)
    {
        MinimumRating = rating;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetKeyword(string keyword)
    {
        Keyword = keyword;
    }

    // The method to perform the actual search
    public List<Recipe> PerformSearch(List<Recipe> allRecipes)
    {
        var query = allRecipes.AsEnumerable();

        if (!string.IsNullOrEmpty(Name))
        {
            query = query.Where(recipe => recipe.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
        }

        if (MinimumRating.HasValue)
        {
            query = query.Where(recipe => recipe.Ratings.Any() && 
                                          recipe.Ratings.Average(rating => rating.Score) >= MinimumRating.Value);
        }

        if (Tags.Any())
        {
            query = query.Where(recipe => recipe.Tags.Any(tag => Tags.Contains(tag)));
        }

        if (Ingredients.Any())
        {
            query = query.Where(recipe => recipe.Ingredients.Any(ingredient => Ingredients.Contains(ingredient.Name)));
        }

        if (!string.IsNullOrEmpty(Keyword))
        {
            query = query.Where(recipe => recipe.Name.Contains(Keyword, StringComparison.OrdinalIgnoreCase) ||
                                          recipe.Tags.Any(tag => tag.Contains(Keyword, StringComparison.OrdinalIgnoreCase)));
        }

        return query.ToList();
    }
}
