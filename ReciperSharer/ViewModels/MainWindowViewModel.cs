﻿using System.Reactive.Linq;
using System;
using ReactiveUI;
using Users;
using Recipes;
using Context;
using System.Reactive;

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
    NavigateToEditRecipeCommand = ReactiveCommand.Create(NavigateToEditRecipe);
    NavigateToEditRecipeIngredientCommand = ReactiveCommand.Create(NavigateToEditRecipeIngredient);
    NavigateToEditRecipeStepCommand = ReactiveCommand.Create(NavigateToEditRecipeStep);
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
    viewModel.ShowMakeRecipeCommand.Subscribe (_4 => NavigateToMakeRecipe() );
    viewModel.ShowProfileCommand.Subscribe (_5 => NavigateToProfile());

    ContentViewModel = viewModel;
  }
   public void NavigateToRecipes()
    {
        RecipesViewModel viewModel = new();
        viewModel.Home.Subscribe(_ => NavigateToLoggedIn());
        viewModel.Edit.Subscribe(recipeId => NavigateToEditRecipe(recipeId));

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

    public void NavigateToMakeRecipe()
    {
        ContentViewModel = new MakeRecipeViewModel();
    }

    public void NavigateToProfile()
    {
      ProfileViewModel viewModel = new();
      viewModel.GoBackCommand.Subscribe (_ => NavigateToLoggedIn());

        ContentViewModel = viewModel;
    }


  private void NavigateToEditRecipe(int? recipeId)
  {
    RecipeEditViewModel viewModel = new(recipeId);
    viewModel.SaveCommand.Subscribe(_ => NavigateToRecipes());
    viewModel.CancelCommand.Subscribe(_ => NavigateToRecipes());
  }

  private void NavigateToEditRecipeIngredient()
  {
    RecipeEditIngredientsViewModel viewModel = new();
    ContentViewModel = viewModel;
  }

  private void NavigateToEditRecipeStep()
  {
    RecipeEditStepsViewModel viewModel = new();
    ContentViewModel = viewModel;
  }
}
