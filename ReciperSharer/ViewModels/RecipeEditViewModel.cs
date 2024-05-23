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
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public string? Title {get;}
    public string _name;
    public string Name { 
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value); }
    private string _shortDescription;
    public string ShortDescription { 
        get => _shortDescription;
        set => this.RaiseAndSetIfChanged(ref _shortDescription, value);
     }
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
    public int _servings;
    public int Servings{
        get => _servings;
        set => this.RaiseAndSetIfChanged(ref _servings, value);
    }
    private Recipe _currentRecipe;
    public Recipe CurrentRecipe
    {
        get => _currentRecipe;
        set => this.RaiseAndSetIfChanged(ref _currentRecipe, value);
    }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    // public ReactiveCommand<Unit, Unit> AddIngredient { get; }
    // public ReactiveCommand<int, Unit> RemoveIngredient { get; }
    // public ReactiveCommand<Unit, Unit> AddStep { get; }
    // public ReactiveCommand<int, Unit> RemoveStep { get; }


    // public ReactiveCommand<Unit, Unit> EditIngredientsCommand { get; }
    // public ReactiveCommand<Unit, Unit> EditInstructionsCommand { get; }


    public RecipeEditViewModel(int? RecipeId)
    {
        if (RecipeId != null){
            try{
                CurrentRecipe = RecipeOperations.INSTANCE!.GetRecipe((int)RecipeId);
                Title = $"Edit Recipe: {Name}";
            }
            catch(Exception e){
                ErrorMessage = e.Message;
            }
            SaveCommand = ReactiveCommand.Create(() => {
                
            })
        }
        else if (RecipeId == null){
            Title = "Create New Recipe";
            CurrentRecipe = new();
        }


    }

    // private void EditIngredients()
    // {
    //     // Navigate to EditRecipeView with CurrentRecipe bound to the view
    // }

    // private void EditInstructions()
    // {

    // }

    // private void SaveEdit()
    // {
    //     _currentRecipe.Owner = CurrentUser;
    //     _currentRecipe.Name = Name;
    //     _currentRecipe.ShortDescription = ShortDescription;
    //     _currentRecipe.PreparationTime = PreparationTime;
    //     _currentRecipe.CookingTime = CookingTime;
    //     _currentRecipe.Servings = Servings;
    //     Recipe updatedRecipe = new Recipe(CurrentUser, _currentRecipe.Name, _currentRecipe.ShortDescription, _currentRecipe.Ingredients, _currentRecipe.PreparationTime, _currentRecipe.CookingTime, _currentRecipe.Servings);
    //     _recipeOperations.UpdateRecipe(_currentUser, _currentRecipe, updatedRecipe);
    //     //_recipeOperations.UpdateRecipe(owner, CurrentRecipe, CurrentRecipe);
    //     // Optionally navigate back or refresh the list
    // }

    // private void CancelEdit()
    // {
    //     // Optionally navigate back or clear CurrentRecipe
    // }
}
