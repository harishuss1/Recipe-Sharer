using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Recipes;
using System.Collections.ObjectModel;
using Users;
namespace RecipeShare.ViewModels;

public class RecipeEditStepsViewModel : ViewModelBase
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
    public ReactiveCommand<Unit, Unit> AddStepCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteStepCommand { get; }
    


   public RecipeEditStepsViewModel()
    {
        //_recipeOperations = recipeOperations;
        //Recipes = new ObservableCollection<Recipe>(_recipeOperations.ViewUserRecipes(/*owner*/));

        AddStepCommand = ReactiveCommand.Create(AddStep);
        DeleteStepCommand = ReactiveCommand.Create(DeleteStep);

        SaveCommand = ReactiveCommand.Create(SaveEdit);
        CancelCommand = ReactiveCommand.Create(CancelEdit);
    }

    private void AddStep()
    {
        
    }

    private void DeleteStep()
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
