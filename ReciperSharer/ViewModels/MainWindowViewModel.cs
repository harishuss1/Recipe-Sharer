﻿using System.Reactive.Linq;
using System;
using ReactiveUI;
using Users;

namespace RecipeShare.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
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
    viewModel.ShowMakeRecipeCommand.Subscribe (_4 => NavigateToMakeRecipe() );
    viewModel.ShowProfileCommand.Subscribe (_5 => NavigateToProfile());

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

}
