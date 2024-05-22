﻿using System.Reactive.Linq;
using System;
using ReactiveUI;
using Users;
using Recipes;
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

    ContentViewModel = viewModel;
  }

  private void NavigateToEditRecipe()
  {
    var recipeOperations = new RecipeOperations(); // Create or get the instance
    var user = new User(); // Create or get the instance
    RecipeEditViewModel viewModel = new(/*recipeOperations, user*/);
    ContentViewModel = viewModel;
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
