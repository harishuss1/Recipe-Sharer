using System;
using System.Reactive;
using RecipeShare.Controllers;
using ReactiveUI;
using Users;
using Context;
using RecipeSharer;

namespace RecipeShare.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    private string _username;
    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }
    private string _description;
    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    private string _message;
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }

    private bool _isAccountDeleted;
    public bool IsAccountDeleted
    {
        get => _isAccountDeleted;
        private set => this.RaiseAndSetIfChanged(ref _isAccountDeleted, value);
    }

    public ReactiveCommand<Unit, Unit> ViewUserRecipesCommand { get; }
    public ReactiveCommand<Unit, Unit> ViewFavoriteRecipesCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteAccountCommand { get; }
    public ReactiveCommand<Unit, Unit> EditProfileCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetPasswordCommand { get; }
    public ReactiveCommand<Unit, Unit> ChangeProfilePictureCommand { get; }
    public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

    private readonly RecipeSharerContext _context;
    private readonly UserServices _userServices;
    private User _currentUser;


    public ProfileViewModel(Action navigateToUserRecipes, Action navigateToFavoriteRecipes)
    {
        // Initialize properties with the current user's data
        _context = RecipeSharerContext.INSTANCE ?? throw new ArgumentNullException(nameof(RecipeSharerContext.INSTANCE));
        _userServices = UserServices.INSTANCE ?? throw new ArgumentNullException(nameof(UserServices.INSTANCE));
        _currentUser = UserController.INSTANCE.CurrentlyLoggedInUser ?? throw new InvalidOperationException("No user is currently logged in");

        Username = _currentUser.Username;
        Description = _currentUser.Description;
        IsAccountDeleted = false;

        // Initialize commands
        DeleteAccountCommand = ReactiveCommand.Create(DeleteAccount);
        EditProfileCommand = ReactiveCommand.Create(EditProfile);
        ResetPasswordCommand = ReactiveCommand.Create(() => {  });
        ChangeProfilePictureCommand = ReactiveCommand.Create(() => {  });
        ViewUserRecipesCommand = ReactiveCommand.Create(navigateToUserRecipes);
        ViewFavoriteRecipesCommand = ReactiveCommand.Create(navigateToFavoriteRecipes);
        GoBackCommand = ReactiveCommand.Create(() => {  });
    }

    private void DeleteAccount()
    {
        try 
        {
            _userServices.DeleteUser(_currentUser.Username);
            Message = "Account deleted successfully.";
            IsAccountDeleted = true;
        }
        catch(Exception e){
            Message = e.Message;
            IsAccountDeleted = false;
        }
    }

    private void EditProfile()
    {
        // Logic to edit user profile
    }

    private void GoBack()
    {
        if (IsAccountDeleted )
        {
            UserController.INSTANCE.Logout();
        }
    }

        // private void ViewUserRecipes()
    // {
    //     // Logic to display user recipes
    // }

    // private void ViewFavoriteRecipes()
    // {
    //     // Logic to display favorite recipes
    // }
}
