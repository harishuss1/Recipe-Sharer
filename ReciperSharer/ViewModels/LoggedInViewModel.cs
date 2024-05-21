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

  public LoggedInViewModel()
  {
    Logout = ReactiveCommand.Create(() => UserController.INSTANCE.Logout());
    Greeting =
      $"Hello {UserController.INSTANCE.CurrentlyLoggedInUser!.Username!}";
  }
}
