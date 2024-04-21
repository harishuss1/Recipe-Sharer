namespace RecipeSharer;
public class Ingredient
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string UnitOfMass { get; set; }
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

        int choice = 0; 

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (input >= 1 && input <= 3)
                {
                    choice = input; 
                    break; 
                }
            }
            Console.WriteLine("Invalid input. Please choose 1, 2, or 3.");
        }

        return choice; 
    }

    public void recipeScaler()
    {
        int multiplier = 0;
        if (multiplier == 0)
        {
            multiplier = GetUserMultiplier();
        }
        //needs the db to loop through ingredients list?
        double scaledQuantity = Quantity * multiplier;
        Console.WriteLine($"Scaled {Name} to {multiplier}x: {scaledQuantity} {UnitOfMass}");
    }

    public static int ConvertUnit(int quantity, string currentUnit, string type)
    {
        switch (currentUnit.ToLower())
        {
            case "cups":
                if (type.ToLower() == "solid")
                {
                    // Convert cups to grams for solid ingredients
                    return (int)(quantity * 236.588);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert cups to milliliters for liquid ingredients
                    return (int)(quantity * 236.588);
                }
                else
                {
                    // Keep cups as is for other types of ingredients
                    return quantity;
                }
            case "tablespoons":
                if (type.ToLower() == "solid")
                {
                    // Convert tablespoons to grams for solid ingredients
                    return (int)(quantity * 14.7868);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert tablespoons to milliliters for liquid ingredients
                    return (int)(quantity * 14.7868);
                }
                else
                {
                    // Keep tablespoons as is for other types of ingredients
                    return quantity;
                }
            case "teaspoons":
                if (type.ToLower() == "solid")
                {
                    // Convert teaspoons to grams for solid ingredients
                    return (int)(quantity * 4.92892);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert teaspoons to milliliters for liquid ingredients
                    return (int)(quantity * 4.92892);
                }
                else
                {
                    // Keep teaspoons as is for other types of ingredients
                    return quantity;
                }
            default:
                return quantity; // Return quantity unchanged for unknown units
        }
    }

    public override string ToString()
    {
        return $"{Name}: {Quantity} {UnitOfMass}";
    }
}