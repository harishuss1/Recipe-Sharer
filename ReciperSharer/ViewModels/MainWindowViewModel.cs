﻿using System.Reactive.Linq;
using System;
using ReactiveUI;
using Users;
using Recipes;
using Context;
using System.Reactive;
using RecipeShare.Controllers;

namespace RecipeShare.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
  public ReactiveCommand<Unit, Unit> NavigateToEditRecipeCommand { get; }
  public ReactiveCommand<Unit, Unit> NavigateToEditRecipeIngredientCommand { get; }
  public ReactiveCommand<Unit, Unit> NavigateToEditRecipeStepCommand { get; }
  private ViewModelBase _contentViewModel;

  public ViewModelBase ContentViewModel
  {
    get => _contentViewModel;
    private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
  }

  public MainWindowViewModel()
  {
    _contentViewModel = new WelcomeViewModel();
  }

  public void NavigateToWelcome()
  {
    ContentViewModel = new WelcomeViewModel();
  }

  public void NavigateToRegister()
  {
    RegisterViewModel viewModel = new();

    viewModel.Register.Subscribe(user =>
    {
      if (user != null)
      {
        NavigateToWelcome();
      }
    });

    ContentViewModel = viewModel;
  }

  public void NavigateToLogin()
  {
    LoginViewModel viewModel = new();

    viewModel.Login.Subscribe(user =>
    {
      if (user != null)
      {
        NavigateToLoggedIn();
      }
    });

    ContentViewModel = viewModel;
  }

  public void NavigateToLoggedIn()
  {
    LoggedInViewModel viewModel = new();

    viewModel.Logout.Subscribe(_ => NavigateToWelcome());
    viewModel.ShowRecipeCommand.Subscribe (_1 => NavigateToRecipes());
    viewModel.ShowRatingCommand.Subscribe (_2 => NavigateToRatings());
    viewModel.ShowSearchCommand.Subscribe (_3 => NavigateToSearch());
    viewModel.ShowMakeRecipeCommand.Subscribe (_4 => NavigateToEditRecipe() );
    viewModel.ShowProfileCommand.Subscribe (_5 => NavigateToProfile());

    ContentViewModel = viewModel;
  }
   public void NavigateToRecipes()
    {
        RecipesViewModel viewModel = new();
        viewModel.Home.Subscribe(_ => NavigateToLoggedIn());
        viewModel.Edit.Subscribe(_ => NavigateToEditRecipe());
        viewModel.NewRecipe.Subscribe(_ => NavigateToEditRecipe());

        ContentViewModel = viewModel;
    }

    public void NavigateToRatings()
    {
        ContentViewModel = new RatingsViewModel();
    }

    public void NavigateToSearch()
    {
        ContentViewModel = new SearchViewModel();
    }

    public void NavigateToProfile()
    {
      ProfileViewModel viewModel = new();
      viewModel.GoBackCommand.Subscribe (_ => NavigateToLoggedIn());

        ContentViewModel = viewModel;
    }


  private void NavigateToEditRecipe()
  {
    RecipeEditViewModel viewModel = new();
    viewModel.SaveCommand.Subscribe(_ => NavigateToRecipes());
    viewModel.CancelCommand.Subscribe(_ => NavigateToRecipes());

    ContentViewModel = viewModel;
  }
}
