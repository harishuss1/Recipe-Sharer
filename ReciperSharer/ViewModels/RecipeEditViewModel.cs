using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Recipes;
using System.Collections.ObjectModel;
using System.Linq;
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
    

    public ObservableCollection<Tag> AllTags {get;}

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    public ReactiveCommand<Unit, Unit> AddIngredient { get; }
    public ReactiveCommand<Ingredient, Unit> RemoveIngredient { get; }
    public ReactiveCommand<Unit, Unit> AddStep { get; }
    public ReactiveCommand<Step, Unit> RemoveStep { get; }
    public ReactiveCommand<Unit, Unit> AddTag { get; }
    public ReactiveCommand<Tag, Unit> RemoveTag { get; }


    public RecipeEditViewModel()
    {
        int? RecipeId = UserController.INSTANCE!.EdittedRecipeId;
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
                                                                    Ingredients.ToList(), 
                                                                    PreparationTime, 
                                                                    CookingTime, 
                                                                    Servings,
                                                                    Steps.ToList(), 
                                                                    Tags.ToList());
                    UserController.INSTANCE!.EditRecipe(CurrentRecipe, newRecipe);
                    UserController.INSTANCE!.EdittedRecipeId = null;
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
                                                                    Ingredients.ToList(), 
                                                                    PreparationTime, 
                                                                    CookingTime, 
                                                                    Servings,
                                                                    Steps.ToList(), 
                                                                    Tags.ToList());
                    UserController.INSTANCE!.CreateRecipe(newRecipe);
                    UserController.INSTANCE!.EdittedRecipeId = null;
                });
                
            }
            catch(Exception e){
                ErrorMessage = e.Message;
            }
        }

        //These are assigned whether editing or adding:
        AllTags = new(RecipeOperations.INSTANCE!.GetAllTags());

        CancelCommand = ReactiveCommand.Create(() => { 
            UserController.INSTANCE!.EdittedRecipeId = null;
        });

        //Ingredients:
        AddIngredient = ReactiveCommand.Create(() => {
            Ingredients.Add(new Ingredient());
        });
        RemoveIngredient = ReactiveCommand.Create<Ingredient>((Ingredient ingredient) => {
            Ingredients.Remove(ingredient);
        });

        //Steps:
        AddStep = ReactiveCommand.Create(() => {
            Step step = new();

            step.Number = Steps.Count+1;
            Steps.Add(step);
        });
        RemoveStep = ReactiveCommand.Create<Step>((Step step) => {
            Steps.Remove(step);
            for (int i=0; i < Steps.Count; i++){
                Steps[i].Number = i+1;
            }
        });

        //Tags:
        AddTag = ReactiveCommand.Create(() => {
            Tags.Add(new Tag());
        });
        RemoveTag = ReactiveCommand.Create<Tag>((Tag tag) => {
            Tags.Remove(tag);
        });

    }
}