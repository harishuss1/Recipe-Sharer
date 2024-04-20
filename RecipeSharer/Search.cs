namespace Search;
using Recipe;
public class Search
{
    public List<string> Tags { get; private set; }
    public double? MinimumRating { get; private set; }
    public string Name { get; private set; }
    public List<string> Ingredients { get; private set; }

    public string Keyword { get; private set; }

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


}
