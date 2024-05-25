using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;
using Recipes;
using System.Collections.ObjectModel;

namespace RecipeShare.ViewModels;

public class MakeRecipeViewModel : ViewModelBase
{

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    private Recipe _currentRecipe;
    public Recipe CurrentRecipe
    {
        get => _currentRecipe;
        set => this.RaiseAndSetIfChanged(ref _currentRecipe, value);
    }

    public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

    public MakeRecipeViewModel()
    {
        int? RecipeId = UserController.INSTANCE!.MakeRecipeId;

        CurrentRecipe = RecipeOperations.INSTANCE!.GetRecipe((int)RecipeId);

        GoBackCommand = ReactiveCommand.Create(() => { });
    }
}