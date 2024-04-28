namespace Recipes;

public class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string UnitOfMass { get; set; }
    public string Type { get; set; }

    // public Ingredient(string name, double quantity, string unitOfMass, string type)
    // {
    //     Name = name;
    //     Quantity = Math.Round(quantity, 2);
    //     UnitOfMass = unitOfMass;
    //     Type = type;
    // }

    public void RecipeScaler(int multiplier)
    {
        if (multiplier <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Multiplier must be greater than zero.");
        }
        
        double scaledQuantity = Quantity * multiplier;
        Console.WriteLine($"Scaled {Name} to {multiplier}x: {scaledQuantity} {UnitOfMass}");
    }

    public static int GetUserMultiplier()
    {
        Console.WriteLine("Choose a multiplier:");
        Console.WriteLine("1. 1x");
        Console.WriteLine("2. 2x");
        Console.WriteLine("3. 3x");

        int choice;
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

    // idk how to convert the unit also ;-; it just does the quantity
    public static double ConvertUnit(double quantity, string currentUnit, string type)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }
        switch (currentUnit.ToLower())
        {
            case "cups":
                if (type.ToLower() == "solid")
                {
                    // Convert cups to grams for solid ingredients
                    return Math.Round(quantity * 236.588, 2);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert cups to milliliters for liquid ingredients
                    return Math.Round(quantity * 236.588, 2);
                }
                else
                {
                    // Keep cups as is for other types of ingredients
                    return Math.Round(quantity, 2); ;
                }
            case "g":
                if (type.ToLower() == "solid")
                {
                    // Convert grams to cups for solid ingredients
                    return Math.Round(quantity / 236.588, 2);
                }
                else
                {
                    // Keep grams as is for other types of ingredients
                    return Math.Round(quantity, 2);
                }
            case "tablespoons":
                if (type.ToLower() == "solid")
                {
                    // Convert tablespoons to grams for solid ingredients
                    return Math.Round(quantity * 14.7868, 2);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert tablespoons to milliliters for liquid ingredients
                    return Math.Round(quantity * 14.7868, 2);
                }
                else
                {
                    // Keep tablespoons as is for other types of ingredients
                    return Math.Round(quantity, 2);
                }
            case "teaspoons":
                if (type.ToLower() == "solid")
                {
                    // Convert teaspoons to grams for solid ingredients
                    return Math.Round(quantity * 4.92892, 2);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert teaspoons to milliliters for liquid ingredients
                    return Math.Round(quantity * 4.92892, 2);
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