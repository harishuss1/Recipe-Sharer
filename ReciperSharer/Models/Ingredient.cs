using System.Collections.Concurrent;

namespace Recipes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public double RecipeScaler(int multiplier)
    {
        if (multiplier <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Multiplier must be greater than zero.");
        }
        
        double scaledQuantity = Quantity * multiplier;
        return scaledQuantity;
    }

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
                quantity *= 236.588;
            }
            else if (type.ToLower() == "liquid")
            {
                // Convert cups to milliliters for liquid ingredients
                currentUnit = "ml";
                quantity *= 236.588;
            }
            break;
        case "tablespoons":
            if (type.ToLower() == "solid")
            {
                // Convert tablespoons to grams for solid ingredients
                currentUnit = "g";
                quantity *= 14.7868;
            }
            else if (type.ToLower() == "liquid")
            {
                // Convert tablespoons to milliliters for liquid ingredients
                currentUnit = "ml";
                quantity *= 14.7868;
            }
            break;
        case "teaspoons":
            if (type.ToLower() == "solid")
            {
                // Convert teaspoons to grams for solid ingredients
                currentUnit = "g";
                quantity *= 4.92892;
            }
            else if (type.ToLower() == "liquid")
            {
                // Convert teaspoons to milliliters for liquid ingredients
                currentUnit = "ml";
                quantity *= 4.92892;
            }
            break;
        default:
            // For unknown units, return quantity unchanged
            break;
        }
        return new Tuple<double, string>(Math.Round(quantity, 2), currentUnit);
    }

    public override string ToString()
    {
        return $"{Name}: {Quantity} {UnitOfMass}";
    }
}