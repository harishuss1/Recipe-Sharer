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

        private string _message;
        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public ReactiveCommand<Recipe, Unit> RemoveFromFavoritesCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        public FavoriteRecipesViewModel(Action goBackAction)
        {
            _userServices = UserServices.INSTANCE ?? throw new ArgumentNullException(nameof(UserServices.INSTANCE));
            _currentUser = UserController.INSTANCE.CurrentlyLoggedInUser ?? throw new InvalidOperationException("No user is currently logged in");

            FavoriteRecipes = _userServices.GetUserFavoriteRecipes(_currentUser.UserId);

            RemoveFromFavoritesCommand = ReactiveCommand.Create<Recipe>(RemoveFromFavorites);
            GoBackCommand = ReactiveCommand.Create(goBackAction);
        }

        private void RemoveFromFavorites(Recipe recipe)
        {
           try
            {
                _userServices.RemoveRecipeFromFavorites(recipe, _currentUser);
                FavoriteRecipes.Remove(recipe);
                Message = $"'{recipe.Name}' has been removed from your favorites.";
            }
            catch (Exception ex)
            {
                Message = $"Error removing recipe: {ex.Message}";
            }
        }
    }
}
