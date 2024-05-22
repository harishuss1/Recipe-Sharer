using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Recipes;
using System.Collections.ObjectModel;
using Users;
namespace RecipeShare.ViewModels;

public class RecipeEditIngredientsViewModel : ViewModelBase
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
    public ReactiveCommand<Unit, Unit> AddIngredientCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteIngredientCommand { get; }
    


   public RecipeEditIngredientsViewModel()
    {
        //_recipeOperations = recipeOperations;
        //Recipes = new ObservableCollection<Recipe>(_recipeOperations.ViewUserRecipes(/*owner*/));

        AddIngredientCommand = ReactiveCommand.Create(AddIngredient);
        DeleteIngredientCommand = ReactiveCommand.Create(DeleteIngredient);

        SaveCommand = ReactiveCommand.Create(SaveEdit);
        CancelCommand = ReactiveCommand.Create(CancelEdit);
    }

    private void AddIngredient()
    {
        
    }

    private void DeleteIngredient()
    {

    }

    private void SaveEdit()
    {
        //_recipeOperations.addIngredient();
        // Optionally navigate back or refresh the list
    }

    private void CancelEdit()
    {
        // Optionally navigate back or clear CurrentRecipe
    }
}
