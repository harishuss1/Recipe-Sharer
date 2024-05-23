using System;
using System.Reactive;
using ReactiveUI;
using Users;
using Context;
using RecipeSharer;
using RecipeShare.Controllers;

namespace RecipeShare.ViewModels;

public class ChangePasswordViewModel : ViewModelBase
{
    private readonly UserServices _userServices;
    private readonly User _currentUser;

    private string _currentPassword;
    public string CurrentPassword
    {
        get => _currentPassword;
        set => this.RaiseAndSetIfChanged(ref _currentPassword, value);
    }

    private string _newPassword;
    public string NewPassword
    {
        get => _newPassword;
        set => this.RaiseAndSetIfChanged(ref _newPassword, value);
    }

    private string _confirmNewPassword;
    public string ConfirmNewPassword
    {
        get => _confirmNewPassword;
        set => this.RaiseAndSetIfChanged(ref _confirmNewPassword, value);
    }

    private string _message;
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }

    public ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public ChangePasswordViewModel()
    {
        _userServices = UserServices.INSTANCE ?? throw new ArgumentNullException(nameof(UserServices.INSTANCE));
        _currentUser = UserController.INSTANCE.CurrentlyLoggedInUser ?? throw new InvalidOperationException("No user is currently logged in");

        ChangePasswordCommand = ReactiveCommand.Create(ChangePassword);
        CancelCommand = ReactiveCommand.Create(() => { });
    }

    private void ChangePassword()
    {
        if (NewPassword != ConfirmNewPassword)
        {
            Message = "New passwords do not match.";
            return;
        }
        try
        {
            bool result = _userServices.ChangePassword(_currentUser, NewPassword, CurrentPassword);
            if (result)
            {
                Message = "Password changed successfully.";
            }
            else
            {
                Message = "Failed to change password.";
            }
        }
        catch (Exception ex)
        {
            Message = ex.Message;
        }
    }
}

