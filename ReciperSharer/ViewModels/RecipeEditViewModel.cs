using System;
using System.Reactive;
using RecipeShare.Controllers;
using RecipeShare.Models;
using ReactiveUI;
namespace RecipeShare.ViewModels;

public class RecipeEditViewModel : ViewModelBase
{
    private Recipe _currentRecipe;
    public Recipe CurrentRecipe
    {
        get => _currentRecipe;
        set => SetProperty(ref _currentRecipe, value);
    }

     public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }


   public RecipeViewModel(RecipeOperations recipeOperations)
    {
        _recipeOperations = recipeOperations;
        Recipes = new ObservableCollection<Recipe>(_recipeOperations.ViewUserRecipes());

        EditRecipeCommand = ReactiveCommand.Create<Recipe>(EditRecipe);

        SaveCommand = ReactiveCommand.Create(SaveEdit);
        CancelCommand = ReactiveCommand.Create(CancelEdit);
    }

    private void EditRecipe(Recipe recipe)
    {
        CurrentRecipe = recipe;
        // Navigate to EditRecipeView with CurrentRecipe bound to the view
    }

    private void SaveEdit()
    {
        _recipeOperations.UpdateRecipe(CurrentRecipe);
        // Optionally navigate back or refresh the list
    }

    private void CancelEdit()
    {
        // Optionally navigate back or clear CurrentRecipe
    }
}
