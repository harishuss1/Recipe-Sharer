using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;

namespace RecipeShare.ViewModels;

public class MakeRecipeViewModel : ViewModelBase 
{

    public ReactiveCommand<Unit, Unit> GoBackCommand { get; }


    public MakeRecipeViewModel()
    {
        GoBackCommand = ReactiveCommand.Create(() => {  });
    }
}