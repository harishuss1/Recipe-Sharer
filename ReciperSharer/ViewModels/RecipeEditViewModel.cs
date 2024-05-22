using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Recipes;
using System.Collections.ObjectModel;
using Users;
namespace RecipeShare.ViewModels;

public class RecipeEditViewModel : ViewModelBase
{
    //private readonly RecipeOperations _recipeOperations;
    //private readonly User owner;
    private Recipe _currentRecipe;
    public Recipe CurrentRecipe
    {
        get => _currentRecipe;
        set => this.RaiseAndSetIfChanged(ref _currentRecipe, value);
    }
    public ObservableCollection<Recipe> Recipes { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    public ReactiveCommand<Unit, Unit> EditIngredientsCommand { get; }
    public ReactiveCommand<Unit, Unit> EditInstructionsCommand { get; }


   public RecipeEditViewModel(/*RecipeOperations recipeOperations, User owner*/)
    {
        //_recipeOperations = recipeOperations;
        //Recipes = new ObservableCollection<Recipe>(_recipeOperations.ViewUserRecipes(/*owner*/));

        EditIngredientsCommand = ReactiveCommand.Create(EditIngredients);
        EditInstructionsCommand = ReactiveCommand.Create(EditInstructions);

        SaveCommand = ReactiveCommand.Create(SaveEdit);
        CancelCommand = ReactiveCommand.Create(CancelEdit);
    }

    private void EditIngredients()
    {
        // Navigate to EditRecipeView with CurrentRecipe bound to the view
    }

    private void EditInstructions()
    {

    }

    private void SaveEdit()
    {
        //_recipeOperations.UpdateRecipe(owner, CurrentRecipe, CurrentRecipe);
        // Optionally navigate back or refresh the list
    }

    private void CancelEdit()
    {
        // Optionally navigate back or clear CurrentRecipe
    }
}
