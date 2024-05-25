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
    private ObservableCollection<Recipe> _recipes;
    public ObservableCollection<Recipe> Recipes {
        get => _recipes;
        set => this.RaiseAndSetIfChanged(ref _recipes, value);
     }

    public ReactiveCommand<int, Unit> Delete { get; set; }
    public ReactiveCommand<int?, Unit> Edit { get; set; }
    
    public ReactiveCommand<Unit, Unit> Home { get; set; }
    public ReactiveCommand<Unit, Unit> NewRecipe { get; set; }

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
                Recipes = new();
            }
        });

        Edit = ReactiveCommand.Create<int?>((int? recipeId) => {
            UserController.INSTANCE!.EdittedRecipeId = recipeId;
        });

        NewRecipe = ReactiveCommand.Create(() => {});
    }
}