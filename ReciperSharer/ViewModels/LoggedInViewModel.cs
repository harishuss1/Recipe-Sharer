using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;
using RecipeSharer;

namespace RecipeShare.ViewModels;

public class LoggedInViewModel : ViewModelBase
{
  public string Greeting { get; }

  public ReactiveCommand<Unit, Unit> Logout { get; }

    public ReactiveCommand<Unit, Unit> ShowRecipeCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowRatingCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowSearchCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowCreateRecipeCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowProfileCommand { get; }

  public LoggedInViewModel()
  {
    Logout = ReactiveCommand.Create(() => UserController.INSTANCE.Logout());
    Greeting =
      $"Hello {UserController.INSTANCE.CurrentlyLoggedInUser!.Username!}";

        ShowRecipeCommand = ReactiveCommand.Create(() => MainWindowViewModel.Instance.NavigateToRecipes());
        ShowRatingCommand = ReactiveCommand.Create(() => MainWindowViewModel.Instance.NavigateToRatings());
        ShowSearchCommand = ReactiveCommand.Create(() => MainWindowViewModel.Instance.NavigateToSearch());
        ShowCreateRecipeCommand = ReactiveCommand.Create(() => MainWindowViewModel.Instance.NavigateToCreateRecipe());
        ShowProfileCommand = ReactiveCommand.Create(() => MainWindowViewModel.Instance.NavigateToProfile());
  }
}
