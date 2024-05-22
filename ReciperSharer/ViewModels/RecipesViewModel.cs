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
    public ObservableCollection<Recipe> Recipes { get; }

    public ReactiveCommand<Recipe, Unit> Delete {get; }
    
    public ReactiveCommand<Unit, Unit> Home { get; }

    public RecipesViewModel()
    {
        Recipes = new(UserController.INSTANCE!.GetUserRecipes());
        Home = ReactiveCommand.Create(() => { });

        Delete = ReactiveCommand.Create<Recipe>((Recipe r) => {
            UserController.INSTANCE!.DeleteRecipe(r);
        });
    }

}