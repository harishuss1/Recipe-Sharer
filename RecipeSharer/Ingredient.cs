using System.Collections.Concurrent;

namespace Recipes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ingredient
{
    [Key]
    public int IngredientId {get; set;}
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string UnitOfMass { get; set; }
    public string Type {get; set;}
    [ForeignKey("RecipeId")]
    public Recipe Recipe {get; set;}

    public Ingredient(string name, double quantity, string unitOfMass, string type)
    {
        Name = name;
        Quantity = Math.Round(quantity, 2);
        UnitOfMass = unitOfMass;
        Type = type;
    }

    public Ingredient(){
        
    }

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
    public static Tuple<double, string> ConvertUnit(double quantity, string currentUnit, string type)
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
                    currentUnit = "g";
                    return new Tuple<double, string> (Math.Round(quantity * 236.588, 2), currentUnit);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert cups to milliliters for liquid ingredients
                    currentUnit = "ml";
                    return new Tuple<double, string> (Math.Round(quantity * 236.588, 2), currentUnit);
                }
                else
                {
                    // Keep cups as is for other types of ingredients
                    return new Tuple<double, string> (Math.Round(quantity, 2), currentUnit);;
                }
            case "g":
                if (type.ToLower() == "solid")
                {
                    // Convert grams to cups for solid ingredients
                    currentUnit = "cups";
                    return new Tuple<double, string> (Math.Round(quantity / 236.588, 2), currentUnit);
                }
                else
                {
                    // Keep grams as is for other types of ingredients
                    return new Tuple<double, string> (Math.Round(quantity , 2), currentUnit);
                }
            case "tablespoons":
                if (type.ToLower() == "solid")
                {
                    // Convert tablespoons to grams for solid ingredients
                    currentUnit = "g";
                    return new Tuple<double, string> (Math.Round(quantity * 14.7868, 2), currentUnit);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert tablespoons to milliliters for liquid ingredients
                    currentUnit = "ml";
                    return new Tuple<double, string> (Math.Round(quantity * 14.7868, 2), currentUnit);
                }
                else
                {
                    // Keep tablespoons as is for other types of ingredients
                    return new Tuple<double, string> (Math.Round(quantity, 2), currentUnit);
                }
            case "teaspoons":
                if (type.ToLower() == "solid")
                {
                    // Convert teaspoons to grams for solid ingredients
                    currentUnit = "g";
                    return new Tuple<double, string> (Math.Round(quantity * 4.92892, 2), currentUnit);
                }
                else if (type.ToLower() == "liquid")
                {
                    // Convert teaspoons to milliliters for liquid ingredients
                    currentUnit = "ml";
                    return new Tuple<double, string> (Math.Round(quantity * 4.92892, 2), currentUnit);
                }
                else
                {
                    // Keep teaspoons as is for other types of ingredients
                    return new Tuple<double, string>(Math.Round(quantity, 2), currentUnit);
                }
            default:
                return new Tuple<double, string>(Math.Round(quantity,2), currentUnit); // Return quantity unchanged for unknown units
        }
    }

    public override string ToString()
    {
        return $"{Name}: {Quantity} {UnitOfMass}";
    }
}