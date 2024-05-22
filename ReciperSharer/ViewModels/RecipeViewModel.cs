using System;
using System.Reactive;
using RecipeShare.Controllers;
using RecipeShare.Models;
using ReactiveUI;
using Recipes;
using Users;
namespace RecipeShare.ViewModels;

public class RecipeViewModel : ViewModelBase
{
    private readonly RecipeOperations _recipeOperations;
    public ObservableCollection<Recipe> Recipes { get; }

    public ReactiveCommand<Recipe, Unit> EditRecipeCommand { get; }
    public ReactiveCommand<Recipe, Unit> DeleteRecipeCommand { get; }

    public RecipeViewModel(RecipeOperations recipeOperations)
    {
        _recipeOperations = recipeOperations ?? throw new ArgumentNullException(nameof(recipeOperations));
        Recipes = new ObservableCollection<Recipe>(_recipeOperations.ViewUserRecipes());

        EditRecipeCommand = ReactiveCommand.Create<Recipe>(EditRecipe);
        DeleteRecipeCommand = ReactiveCommand.Create<Recipe>(DeleteRecipe);
    }

    private void EditRecipe(Recipe recipe)
    {
        // Implement your logic to edit a recipe here.
        // You can use _recipeOperations.UpdateRecipe() method.
    }

    private void DeleteRecipe(Recipe recipe)
    {
        // Implement your logic to delete a recipe here.
        // You can use _recipeOperations.RemoveRecipe() method.
    }
}
