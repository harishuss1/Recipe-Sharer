namespace RecipeSharer;
public class Ingredient
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    private string UnitOfMass { get; set; }
    public string Type {get; set;}

    public Ingredient(string name, int quantity, string unitOfMass, string type)
    {
        Name = name;
        Quantity = quantity;
        UnitOfMass = unitOfMass;
        Type = type;
    }

    public static int GetUserMultiplier()
    {
        Console.WriteLine("Choose a multiplier:");
        Console.WriteLine("1. 1x");
        Console.WriteLine("2. 2x");
        Console.WriteLine("3. 3x");

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice >= 1 && choice <= 3)
                {
                    return choice;
                }
            }
            Console.WriteLine("Invalid input. Please choose 1, 2, or 3.");
        }
    }

    public void recipeScaler()
    {
        int multiplier = GetUserMultiplier();
        if (multiplier != 0)
        {
            double scaledQuantity = Quantity * multiplier;
            Console.WriteLine($"Scaled {Name} to {multiplier}x: {scaledQuantity} {UnitOfMass}");
        }
        else
        {
            Console.WriteLine("Invalid input. Please choose 1, 2, or 3.");
        }
    }
}