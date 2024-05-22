using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Recipes;
using System.Collections.ObjectModel;
using Users;
using Context;
namespace RecipeShare.ViewModels;

public class RecipeEditViewModel : ViewModelBase
{
    
    private User _currentUser;
    public User CurrentUser {
        get => _currentUser;
        set => this.RaiseAndSetIfChanged(ref _currentUser, value);
    }

    public string Name { get; set; }
    public string ShortDescription { get; set; }
    private TimeSpan _preparationTime;
    public TimeSpan PreparationTime
    {
        get => _preparationTime;
        set => this.RaiseAndSetIfChanged(ref _preparationTime, value);
    }
    private TimeSpan _cookingTime;
    public TimeSpan CookingTime
    {
        get => _cookingTime;
        set => this.RaiseAndSetIfChanged(ref _cookingTime, value);
    }
    public TimeSpan TotalTime => PreparationTime + CookingTime;
    public int Servings { get; set; }
    private Recipe _currentRecipe;
    public Recipe CurrentRecipe
    {
        get => _currentRecipe;
        set => this.RaiseAndSetIfChanged(ref _currentRecipe, value);
    }
    private readonly RecipeSharerContext _recipeSharerContext;
    private readonly RecipeOperations _recipeOperations;
    public ObservableCollection<Recipe> Recipes { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    public ReactiveCommand<Unit, Unit> EditIngredientsCommand { get; }
    public ReactiveCommand<Unit, Unit> EditInstructionsCommand { get; }


   public RecipeEditViewModel(RecipeSharerContext recipeSharerContext, RecipeOperations recipeOperations, Recipe currentRecipe)
    {
        _recipeSharerContext = recipeSharerContext;
        _recipeOperations = recipeOperations;
        _currentRecipe = currentRecipe;
        //Recipes = new ObservableCollection<Recipe>(_recipeOperations.ViewUserRecipes(/*owner*/));

        CurrentUser = _currentRecipe.Owner;
        Name = _currentRecipe.Name;
        ShortDescription = _currentRecipe.ShortDescription;
        PreparationTime = _currentRecipe.PreparationTime;
        CookingTime = _currentRecipe.CookingTime;
        Servings = _currentRecipe.Servings;

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
        _currentRecipe.Owner = CurrentUser;
        _currentRecipe.Name = Name;
        _currentRecipe.ShortDescription = ShortDescription;
        _currentRecipe.PreparationTime = PreparationTime;
        _currentRecipe.CookingTime = CookingTime;
        _currentRecipe.Servings = Servings;
        Recipe updatedRecipe = new Recipe(CurrentUser, _currentRecipe.Name, _currentRecipe.ShortDescription, _currentRecipe.Ingredients, _currentRecipe.PreparationTime, _currentRecipe.CookingTime, _currentRecipe.Servings);
        _recipeOperations.UpdateRecipe(_currentUser, _currentRecipe, updatedRecipe);
        //_recipeOperations.UpdateRecipe(owner, CurrentRecipe, CurrentRecipe);
        // Optionally navigate back or refresh the list
    }

    private void CancelEdit()
    {
        // Optionally navigate back or clear CurrentRecipe
    }
}
