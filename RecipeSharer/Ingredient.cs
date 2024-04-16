public class Ingredient
{
    public string Name { get; set; }
    public string Quantity { get; set; }
    public string Type {get; set;}

    public Ingredient(string name, string quantity,string type)
    {
        Name = name;
        Quantity = quantity;
        Type = type;
    }
}