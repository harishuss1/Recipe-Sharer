using System;
using System.Reactive;
using RecipeShare.Controllers;
using RecipeShare.Models;
using ReactiveUI;

namespace RecipeShare.ViewModels;

public class LoginViewModel : ViewModelBase
{
  private string? _username;
  public string? Username
  {
    get => _username;
    set => this.RaiseAndSetIfChanged(ref _username, value);
  }

  private string? _password;
  public string? Password
  {
    get => _password;
    set => this.RaiseAndSetIfChanged(ref _password, value);
  }

  private string? _errorMessage;
  public string? ErrorMessage
  {
    get => _errorMessage;
    set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
  }

  public ReactiveCommand<Unit, User?> Login { get; }

  public LoginViewModel()
  {
    IObservable<bool> areBothFilledIn = this.WhenAnyValue(
      loginViewModel => loginViewModel.Username,
      loginViewModel => loginViewModel.Password,
      (username, password) =>
        !(string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(password))
    );

    Login = ReactiveCommand.Create(() =>
    {
      // we know both values are not null, because of `areBothFilledIn`
      User? loggedIn = UserController.INSTANCE.Login(Username!, Password!);

      if (loggedIn == null)
      {
        ErrorMessage = "Invalid username or password";
      }

      return loggedIn;
    }, areBothFilledIn);
  }
}
