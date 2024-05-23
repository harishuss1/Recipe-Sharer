using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Recipes;
using System.Collections.ObjectModel;
using Users;
using Context;
namespace RecipeShare.ViewModels;

public class RecipeEditViewModel : ViewModelBase
{
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public string? Title {get;}
    public string _name;
    public string Name { 
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value); }
    private string _shortDescription;
    public string ShortDescription { 
        get => _shortDescription;
        set => this.RaiseAndSetIfChanged(ref _shortDescription, value);
     }
    private TimeSpan _preparationTime;
    public TimeSpan PreparationTime
    {
        get => _preparationTime;
        set => this.RaiseAndSetIfChanged(ref _preparationTime, value);
    }
    private TimeSpan _cookingTime;
    public TimeSpan CookingTime
    {
        get => _cookingTime;
        set => this.RaiseAndSetIfChanged(ref _cookingTime, value);
    }
    public TimeSpan TotalTime => PreparationTime + CookingTime;
    public int _servings;
    public int Servings{
        get => _servings;
        set => this.RaiseAndSetIfChanged(ref _servings, value);
    }
    private Recipe _currentRecipe;
    public Recipe CurrentRecipe
    {
        get => _currentRecipe;
        set => this.RaiseAndSetIfChanged(ref _currentRecipe, value);
    }

    private ObservableCollection<Ingredient> _ingredients;
    public ObservableCollection<Ingredient> Ingredients {
        get => _ingredients;
        set => this.RaiseAndSetIfChanged(ref _ingredients, value);
    }

    private ObservableCollection<Step> _steps;
    public ObservableCollection<Step> Steps {
        get => _steps;
        set => this.RaiseAndSetIfChanged(ref _steps, value);
    }

    private ObservableCollection<Tag> _tags;
    public ObservableCollection<Tag> Tags {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    public ReactiveCommand<Unit, Unit> AddIngredient { get; }
    public ReactiveCommand<int, Unit> RemoveIngredient { get; }
    public ReactiveCommand<Unit, Unit> AddStep { get; }
    public ReactiveCommand<int, Unit> RemoveStep { get; }
    public ReactiveCommand<Unit, Unit> AddTag { get; }
    public ReactiveCommand<int, Unit> RemoveTag { get; }


    public RecipeEditViewModel(int? RecipeId)
    {
        //Id isn't null, so edit recipe
        if (RecipeId != null){
            try{
                CurrentRecipe = RecipeOperations.INSTANCE!.GetRecipe((int)RecipeId);
                Title = $"Edit Recipe: {Name}";

                //Insert current Values into the fields
                Name = CurrentRecipe.Name;
                ShortDescription = CurrentRecipe.ShortDescription;
                PreparationTime = CurrentRecipe.PreparationTime;
                CookingTime = CurrentRecipe.CookingTime;
                Servings = CurrentRecipe.Servings;
                Ingredients = CurrentRecipe.OIngredients;
                Steps = CurrentRecipe.OSteps;
                Tags = CurrentRecipe.OTags;

                
                SaveCommand = ReactiveCommand.Create(() => {
                    Recipe newRecipe = new(UserController.INSTANCE!.CurrentlyLoggedInUser,
                                                                    Name, 
                                                                    ShortDescription, 
                                                                    Ingredients, 
                                                                    PreperationTime, 
                                                                    CookingTime, 
                                                                    Servings);
                    UserController.INSTANCE!.EditRecipe(CurrentRecipe, newRecipe);
                });
            }
            catch(Exception e){
                ErrorMessage = e.Message;
            }
            
        }

        // Id is null, so create new recipe 
        else {
            try {
                Title = "Create New Recipe";
                CurrentRecipe = new();

                Ingredients = new();
                Steps = new();
                Tags= new();

                SaveCommand = ReactiveCommand.Create(() => {
                    Recipe newRecipe = new(UserController.INSTANCE!.CurrentlyLoggedInUser,
                                                                    Name, 
                                                                    ShortDescription, 
                                                                    Ingredients, 
                                                                    PreperationTime, 
                                                                    CookingTime, 
                                                                    Servings);
                    UserController.INSTANCE!.CreateRecipe(newRecipe);
                });
                
            }
            catch(Exception e){
                ErrorMessage = e.Message;
            }
        }

        //These are assigned whether editing or adding:
        CancelCommand = ReactiveCommand.Create(() => { });

        //Ingredients:
        AddIngredient = ReactiveCommand.Create(() => {
            Ingredients.Add(new Ingredient());
        });
        RemoveIngredient = ReactiveCommand.Create((index) => {
            Ingredients.RemoveAt(index);
        });

        //Steps:
        AddStep = ReactiveCommand.Create(() => {
            Steps.Add(new Step());
            Steps[-1].Number = Steps.Count;
        });
        RemoveStep = ReactiveCommand.Create((index) => {
            Steps.RemoveAt(index);
            for (int i=0; i < Steps.Count; i++){
                Steps[i].Number = i+1;
            }
        });

        //Tags:
        AddTag = ReactiveCommand.Create(() => {
            Tags.Add(new Tag());
        });
        RemoveTag = ReactiveCommand.Create((index) => {
            Tags.RemoveAt(index);
        });

    }
}