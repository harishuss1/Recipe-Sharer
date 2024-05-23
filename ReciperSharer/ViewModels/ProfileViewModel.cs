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

    private string _message;
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
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


    public ProfileViewModel()
    {
        // Initialize properties with the current user's data
        _context = RecipeSharerContext.INSTANCE ?? throw new ArgumentNullException(nameof(RecipeSharerContext.INSTANCE));
        _userServices = UserServices.INSTANCE ?? throw new ArgumentNullException(nameof(UserServices.INSTANCE));
        //_currentUser = UserController.INSTANCE.CurrentlyLoggedInUser!.Username!;

        _currentUser = UserController.INSTANCE.CurrentlyLoggedInUser ?? throw new InvalidOperationException("No user is currently logged in");
        Username = _currentUser.Username;
        Description = _currentUser.Description;
        // Initialize commands
        DeleteAccountCommand = ReactiveCommand.Create(DeleteAccount);
        EditProfileCommand = ReactiveCommand.Create(EditProfile);

        ResetPasswordCommand = ReactiveCommand.Create(() => {  });
        ChangeProfilePictureCommand = ReactiveCommand.Create(() => {  });
        GoBackCommand = ReactiveCommand.Create(() => {  });
    }



    private void DeleteAccount()
    {
        try 
        {
            _userServices.DeleteUser(_currentUser.Username);
            Message = "Account deleted successfully.";
        }
        catch(Exception e){
            Message = e.Message;
        }
        // bool isDeleted = _userServices.DeleteUser(_currentUser.Username);
        // // bool isDeleted = UserServices.INSTANCE.DeleteUser(_username);
        // if(isDeleted)
        // {
        //     Message = "Account deleted successfully.";
        // }
        // else
        // {
        //         Message = "Failed to delete account.";
        // }
    }

    private void EditProfile()
    {
        // Logic to edit user profile
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
