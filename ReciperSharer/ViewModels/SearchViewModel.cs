using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;
using Recipes;
using RecipeSearch;
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

    private readonly Search _search;

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    public SearchViewModel()
    {               
        _search = Search.INSTANCE;
        SearchCommand = ReactiveCommand.Create(SearchButton);
    }

    private void SearchButton()
    {
        try
    {   
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


}