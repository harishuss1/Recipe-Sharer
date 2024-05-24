using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using Recipes;
using Users;
using Context;
using RecipeShare.Controllers;
using RecipeSharer;

namespace RecipeShare.ViewModels
{
    public class FavoriteRecipesViewModel : ViewModelBase
    {
        private readonly UserServices _userServices;
        private readonly User _currentUser;

        private List<Recipe> _favoriteRecipes;
        public List<Recipe> FavoriteRecipes
        {
            get => _favoriteRecipes;
            set => this.RaiseAndSetIfChanged(ref _favoriteRecipes, value);
        }

        public ReactiveCommand<Recipe, Unit> ViewRecipeCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        public FavoriteRecipesViewModel(Action<Recipe> viewRecipeAction, Action goBackAction)
        {
            _userServices = UserServices.INSTANCE ?? throw new ArgumentNullException(nameof(UserServices.INSTANCE));
            _currentUser = UserController.INSTANCE.CurrentlyLoggedInUser ?? throw new InvalidOperationException("No user is currently logged in");

            FavoriteRecipes = _userServices.GetUserFavoriteRecipes(_currentUser.UserId);

            ViewRecipeCommand = ReactiveCommand.Create(viewRecipeAction);
            GoBackCommand = ReactiveCommand.Create(goBackAction);
        }
    }
}
