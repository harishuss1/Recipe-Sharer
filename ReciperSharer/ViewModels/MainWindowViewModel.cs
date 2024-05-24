﻿using System.Reactive.Linq;
using System;
using ReactiveUI;
using Users;
<<<<<<< HEAD
using Recipes;
using Context;
using System.Reactive;
using RecipeShare.Controllers;
=======
using Context;
using RecipeSharer;
>>>>>>> b3aa8a93016be885bf7d1e253a1ccfd870163f2f

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
<<<<<<< HEAD
    viewModel.ShowRecipeCommand.Subscribe (_1 => NavigateToRecipes());
    viewModel.ShowRatingCommand.Subscribe (_2 => NavigateToRatings());
    viewModel.ShowSearchCommand.Subscribe (_3 => NavigateToSearch());
    viewModel.ShowMakeRecipeCommand.Subscribe (_4 => NavigateToEditRecipe() );
    viewModel.ShowProfileCommand.Subscribe (_5 => NavigateToProfile());
=======
    viewModel.ShowRecipeCommand.Subscribe(_1 => NavigateToRecipes());
    viewModel.ShowRatingCommand.Subscribe(_2 => NavigateToRatings());
    viewModel.ShowSearchCommand.Subscribe(_3 => NavigateToSearch());
    viewModel.ShowMakeRecipeCommand.Subscribe(_4 => NavigateToMakeRecipe());
    viewModel.ShowProfileCommand.Subscribe(_5 => NavigateToProfile());
>>>>>>> b3aa8a93016be885bf7d1e253a1ccfd870163f2f

    ContentViewModel = viewModel;
  }
  public void NavigateToRecipes()
  {
    RecipesViewModel viewModel = new();
    viewModel.Home.Subscribe(_ => NavigateToLoggedIn());

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
    ContentViewModel = viewModel;
  }

  public void NavigateToMakeRecipe()
  {
    ContentViewModel = new MakeRecipeViewModel();
  }

  public void NavigateToProfile()
  {
    ProfileViewModel viewModel = new(() => NavigateToRecipes());
    viewModel.GoBackCommand.Subscribe(_ =>
    {
<<<<<<< HEAD
        RecipesViewModel viewModel = new();
        viewModel.Home.Subscribe(_ => NavigateToLoggedIn());
        viewModel.Edit.Subscribe(_ => NavigateToEditRecipe());
        viewModel.NewRecipe.Subscribe(_ => NavigateToEditRecipe());
=======
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
>>>>>>> b3aa8a93016be885bf7d1e253a1ccfd870163f2f

  public void NavigateToChangePassword()
  {
    ChangePasswordViewModel viewModel = new();
    viewModel.CancelCommand.Subscribe(_ => NavigateToProfile());
    ContentViewModel = viewModel;
  }

<<<<<<< HEAD
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
=======
  public void NavigateToUpdateUserBio()
  {
    UpdateUserBioViewModel viewModel = new();
    viewModel.CancelCommand.Subscribe(_ => NavigateToProfile());
    viewModel.SaveCommand.Subscribe(_ => NavigateToProfile());
    ContentViewModel = viewModel;
  }
}
>>>>>>> b3aa8a93016be885bf7d1e253a1ccfd870163f2f
