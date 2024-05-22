using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;
using Recipes;
using System.Collections.ObjectModel;

namespace RecipeShare.ViewModels;

public class RecipesViewModel : ViewModelBase
{

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public ObservableCollection<Recipe> Recipes { get; set; }

    public ReactiveCommand<int, Unit> Delete {get; }
    
    public ReactiveCommand<Unit, Unit> Home { get; }

    public RecipesViewModel()
    {   
        try {
            Recipes = new(UserController.INSTANCE!.GetUserRecipes());
        }
        catch(Exception e){
                ErrorMessage = e.Message;
        }
        Home = ReactiveCommand.Create(() => { });

        Delete = ReactiveCommand.Create<int>((int recipeId) => {
            try {
                UserController.INSTANCE!.DeleteRecipe(recipeId);
                Recipes = new(UserController.INSTANCE!.GetUserRecipes());
            }
            catch(Exception e){
                ErrorMessage = e.Message;
            }
        });
    }
}