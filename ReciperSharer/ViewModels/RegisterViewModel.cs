using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;

namespace RecipeShare.ViewModels;

public class RegisterViewModel : ViewModelBase
{
  private User UserToRegister { get; } = new();

  private string? _username;

  public string? Userame
  {
    get => _username;
    set
    {
      UserToRegister.Username = value;

      this.RaiseAndSetIfChanged(ref _username, value);
    }
  }

  private string? _password;
  public string? Password
  {
    get => _password;
    set
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(Password));
      }
      else if (value.Length < UserController.MIN_PASSWORD_LENGTH
        || value.Length > UserController.MAX_PASSWORD_LENGTH)
      {
        throw new ArgumentException($"Must be at least " +
          $"{UserController.MIN_PASSWORD_LENGTH} and at most " +
          $"{UserController.MAX_PASSWORD_LENGTH} characters long",
          nameof(Password));
      }

      this.RaiseAndSetIfChanged(ref _password, value);
    }
  }

  private string? _confirmPassword;
  public string? ConfirmPassword
  {
    get => _confirmPassword;
    set
    {
      if (!string.Equals(value, Password))
      {
        throw new ArgumentException("Must match the first password",
          nameof(ConfirmPassword));
      }

      this.RaiseAndSetIfChanged(ref _confirmPassword, value);
    }
  }

  // private string? _displayName;
  // public string? DisplayName
  // {
  //   get => _displayName;
  //   set
  //   {
  //     UserToRegister.DisplayName = value;

  //     this.RaiseAndSetIfChanged(ref _displayName, value);
  //   }
  // }

  private string? _errorMessage;
  public string? ErrorMessage
  {
    get => _errorMessage;
    set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
  }

  public ReactiveCommand<Unit, User?> Register { get; }

  public RegisterViewModel()
  {
    Register = ReactiveCommand.Create(() =>
    {
      try
      {
        UserController.Register(UserToRegister, Password!);
      }
      catch (Exception exc)
      when (exc is ArgumentException || exc is NullReferenceException)
      {
        ErrorMessage = exc.Message;
        return null;
      }

      return UserToRegister;
    });
  }
}
