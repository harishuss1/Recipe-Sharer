using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Context;
using RecipeSharer;
using Users;

namespace RecipeShare.Controllers;

public class UserController
{
  private static UserController? _instance;
  /// <summary>
  /// Only instance of the UserController class, for singleton design pattern.
  /// </summary>
  public static UserController INSTANCE
  {
    get => _instance ??= new();
  }

  /// <summary>
  /// Prevents further object creation (other than inside INSTANCE)
  /// </summary>
  private UserController() { }

  /// <summary>
  /// Currently (during this current execution) logged-in user. Is null if no
  /// user is logged-in.
  /// </summary>
  public User? CurrentlyLoggedInUser { get; private set; }
//   private UserServices UServ = new UserServices(RecipeSharerContext.INSTANCE!);

  private static string ComputeHash(string password)
  {
    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
    byte[] hashBytes = SHA256.HashData(passwordBytes);

    return Convert.ToHexString(hashBytes);
  }

  private static User? GetUser(string username)
  {
    return new UserServices(RecipeSharerContext.INSTANCE!).GetUser(username);
  }

  public static List<User> GetAllUsers()
  {
    return new UserServices(RecipeSharerContext.INSTANCE!).GetUsers();
  }

  public const int MIN_PASSWORD_LENGTH = 5;

  public const int MAX_PASSWORD_LENGTH = 100;

  /// <summary>
  /// Registers a new user, given the user information (minus the hashed
  /// password) as well as the clear-text password.
  ///
  /// Validates that required fields are set, and that the username isn't
  /// already taken.
  /// </summary>
  /// <exception cref="ArgumentException">
  /// If the user argument is missing one of its properties.
  /// </exception>
  /// <returns>
  /// The newly registered user (i.e. the user argument, with its generated id).
  /// </returns>
  public static User Register(User user, string password)
  {
    if (user.Username == null){
        throw new ArgumentException("Please enter a username");
    }
    user = new UserServices(RecipeSharerContext.INSTANCE!).AddUser(user.Username, password);


    return user;
  }

  /// <summary>
  /// Logs in the user with a given username and password, only if both match
  /// and there isn't already a logged-in user.
  /// </summary>
  /// <exception cref="InvalidOperationException">
  /// If a user is already logged-in.
  /// </exception> 
  /// <returns>
  /// The value of CurrentlyLoggedInUser after the login attempt (could be null
  /// if that failed).
  /// </returns>
  public User? Login(string username, string password)
  {
    if (CurrentlyLoggedInUser != null)
    {
      throw new InvalidOperationException("User already logged in");
    }

    if (new UserServices(RecipeSharerContext.INSTANCE!).UserLogin(username, password))
    {
      CurrentlyLoggedInUser = GetUser(username);
    }

    return CurrentlyLoggedInUser;
  }

  public void Logout()
  {
    CurrentlyLoggedInUser = null;
  }
}
