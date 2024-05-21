using System.Collections.ObjectModel;
using RecipeShare.Controllers;
using Users;

namespace RecipeShare.ViewModels;

public class WelcomeViewModel : ViewModelBase
{
  public ObservableCollection<User> Users { get; }

  public WelcomeViewModel()
  {
    Users = new(UserController.GetAllUsers());
  }
}
