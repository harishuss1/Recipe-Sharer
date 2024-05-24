using System;
using System.Reactive;
using ReactiveUI;
using Recipes;
using Users;
using Context;
using RecipeSharer;

namespace RecipeShare.ViewModels
{
    public class FavoriteRecipeDetailsViewModel : ViewModelBase
    {
        private Recipe _recipe;

        public Recipe Recipe
        {
            get => _recipe;
            set => this.RaiseAndSetIfChanged(ref _recipe, value);
        }

        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        public FavoriteRecipeDetailsViewModel(Recipe recipe, Action goBackAction)
        {
            Recipe = recipe ?? throw new ArgumentNullException(nameof(recipe));
            GoBackCommand = ReactiveCommand.Create(goBackAction);
        }
    }
}
