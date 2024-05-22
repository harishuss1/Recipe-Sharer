using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;

namespace RecipeShare.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    private string _username;
    private string _description;

    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }


    public ReactiveCommand<Unit, Unit> ViewUserRecipesCommand { get; }
    public ReactiveCommand<Unit, Unit> ViewFavoriteRecipesCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteAccountCommand { get; }
    public ReactiveCommand<Unit, Unit> EditProfileCommand { get; }
    public ReactiveCommand<Unit, Unit> GoBackCommand { get; }


    public ProfileViewModel()
    {
        // Initialize properties with the current user's data
        var currentUser = UserController.INSTANCE.CurrentlyLoggedInUser;
        Username = currentUser.Username;
        Description = currentUser.Description;  

        // Initialize commands
        ViewUserRecipesCommand = ReactiveCommand.Create(ViewUserRecipes);
        ViewFavoriteRecipesCommand = ReactiveCommand.Create(ViewFavoriteRecipes);
        DeleteAccountCommand = ReactiveCommand.Create(DeleteAccount);
        EditProfileCommand = ReactiveCommand.Create(EditProfile);
        GoBackCommand = ReactiveCommand.Create(() => MainWindowViewModel.Instance.NavigateToLoggedIn());


    }
    private void GoBack()
    {
        MainWindowViewModel.Instance.ContentViewModel = new LoggedInViewModel();
    }

    private void UpdateProfile()
    {
        // Update the user's profile here
    }

    private void ViewUserRecipes()
    {
        // Logic to display user recipes
    }

    private void ViewFavoriteRecipes()
    {
        // Logic to display favorite recipes
    }

    private void DeleteAccount()
    {
        // Logic to delete user account
    }

    private void EditProfile()
    {
        // Logic to edit user profile
    }
}
