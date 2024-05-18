using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;

namespace RecipeShare.ViewModels;

public class LoggedInViewModel : ViewModelBase
{
  public string Greeting { get; }

  public ReactiveCommand<Unit, Unit> Logout { get; }

  public LoggedInViewModel()
  {
    Logout = ReactiveCommand.Create(() => UserServices.INSTANCE.Logout());
    Greeting =
      $"Hello {UserServices.INSTANCE.CurrentlyLoggedInUser!.DisplayName!}";
  }
}
