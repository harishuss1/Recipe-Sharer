using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;
using Recipes;
using Context;
using RecipeSharer;
using RecipeSearch;
using System.Linq;
using System.Collections.ObjectModel;

namespace RecipeShare.ViewModels;

public class SearchViewModel : ViewModelBase
{
    private ObservableCollection<Recipe> _searchResults = new ObservableCollection<Recipe>();
    public ObservableCollection<Recipe> SearchResults
    {
        get => _searchResults;
        set => this.RaiseAndSetIfChanged(ref _searchResults, value);
    }

    private string _keyword;
    public string Keyword
    {
        get => _keyword;
        set => this.RaiseAndSetIfChanged(ref _keyword, value);
    }
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    private TimeSpan? _minDuration;
    public TimeSpan? MinDuration
    {
        get => _minDuration;
        set => this.RaiseAndSetIfChanged(ref _minDuration, value);
    }

    private TimeSpan? _maxDuration;
    public TimeSpan? MaxDuration
    {
        get => _maxDuration;
        set => this.RaiseAndSetIfChanged(ref _maxDuration, value);
    }

    private double? _minimumRating;
    public double? MinimumRating
    {
        get => _minimumRating;
        set => this.RaiseAndSetIfChanged(ref _minimumRating, value);
    }

    private int? _maxServings;
    public int? MaxServings
    {
        get => _maxServings;
        set => this.RaiseAndSetIfChanged(ref _maxServings, value);
    }

    private int? _minServings;
    public int? MinServings
    {
        get => _minServings;
        set => this.RaiseAndSetIfChanged(ref _minServings, value);
    }

    private string _ownerUsername;
    public string OwnerUsername
    {
        get => _ownerUsername;
        set => this.RaiseAndSetIfChanged(ref _ownerUsername, value);
    }

    private readonly Search _search;
    private readonly UserServices _userServices;
    private readonly User _currentUser;

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

    public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetFilter { get; }
     public ReactiveCommand<Recipe, Unit> AddToFavoritesCommand { get; }

    public ReactiveCommand<Recipe, Unit> RateCommand { get; }

    public SearchViewModel()
    {
        _search = Search.INSTANCE;
        _userServices = UserServices.INSTANCE ?? throw new ArgumentNullException(nameof(UserServices.INSTANCE));
        _currentUser = UserController.INSTANCE.CurrentlyLoggedInUser ?? throw new InvalidOperationException("No user is currently logged in");

        SearchCommand = ReactiveCommand.Create(SearchButton);
        GoBackCommand = ReactiveCommand.Create(() => { });
        ResetFilter = ReactiveCommand.Create(() => { });
        RateCommand = ReactiveCommand.Create<Recipe>((Recipe recipe) => {
            try {UserController.INSTANCE.RateRecipe(recipe);}
            catch (Exception e) {ErrorMessage = e.Message;}
        });
        AddToFavoritesCommand = ReactiveCommand.Create<Recipe>(AddToFavorites);

    }

    private void SearchButton()
    {
        try
        {
            if (MinDuration != null)
            {
                _search.SetTimeConstraints(MinDuration, MaxDuration);
            }

            if (MinimumRating != null)
            {
                _search.SetMinimumRating(MinimumRating);
            }

            if (MinServings != null || MaxServings != null)
            {
                _search.SetServingsConstraints(MinServings, MaxServings);
            }

            if (!string.IsNullOrEmpty(OwnerUsername))
            {
                _search.SetOwnerUsername(OwnerUsername);
            }

            if (Keyword != null)
            {
                _search.SetKeyword(Keyword);
            }

            var results = _search.PerformSearch();

            SearchResults.Clear();

            foreach (var result in results)
            {
                SearchResults.Add(result);
            }
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    private void AddToFavorites(Recipe recipe)
    {
        try
        {
            if (_currentUser.UserFavouriteRecipes.Any(r => r.RecipeId == recipe.RecipeId))
            {
                ErrorMessage = "Recipe is already in your favorites.";
                return;
            }
            _userServices.AddToFavorites(recipe, _currentUser);
            ErrorMessage = "Recipe added to favorites.";
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }
}