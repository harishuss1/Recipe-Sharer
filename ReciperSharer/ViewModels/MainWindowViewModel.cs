﻿using System.Reactive.Linq;
using System;
using ReactiveUI;
using Users;
using Recipes;
using Context;
using System.Reactive;
using RecipeShare.Controllers;
using RecipeSharer;


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
    SearchViewModel viewModel = new();
    viewModel.GoBackCommand.Subscribe(_ => NavigateToLoggedIn());
    viewModel.ResetFilter.Subscribe(_1 => NavigateToSearch());
    ContentViewModel = viewModel;
  }

  public void NavigateToMakeRecipe()
  {
    MakeRecipeViewModel viewModel = new ();
    viewModel.GoBackCommand.Subscribe(_ => NavigateToLoggedIn());
    ContentViewModel = viewModel;
  }

  public void NavigateToProfile()
  {
    ProfileViewModel viewModel = new(() => NavigateToRecipes());
    viewModel.GoBackCommand.Subscribe(_ =>
    {

      if (viewModel.IsAccountDeleted)
      {
          NavigateToWelcome();
      }
      else
      {
          NavigateToLoggedIn();
      }
    });
    viewModel.EditProfileCommand.Subscribe(_ => NavigateToUpdateUserBio());
    ContentViewModel = viewModel;
  }

  public void NavigateToChangePassword()
  {
    ChangePasswordViewModel viewModel = new();
    viewModel.CancelCommand.Subscribe(_ => NavigateToProfile());
    ContentViewModel = viewModel;
  }

  private void NavigateToEditRecipe()
  {
    RecipeEditViewModel viewModel = new();
    viewModel.SaveCommand.Subscribe(_ => NavigateToRecipes());
    viewModel.CancelCommand.Subscribe(_ => NavigateToRecipes());

    ContentViewModel = viewModel;
  }


  public void NavigateToUpdateUserBio()
  {
    UpdateUserBioViewModel viewModel = new();
    viewModel.CancelCommand.Subscribe(_ => NavigateToProfile());
    viewModel.SaveCommand.Subscribe(_ => NavigateToProfile());
    ContentViewModel = viewModel;
  }
}

