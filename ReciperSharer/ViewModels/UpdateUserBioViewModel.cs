using System;
using System.Reactive;
using ReactiveUI;
using Users;
using RecipeShare.Controllers;
using RecipeSharer;

namespace RecipeShare.ViewModels
{
    public class UpdateUserBioViewModel : ViewModelBase
    {
        private readonly UserServices _userServices;
        private readonly User _currentUser;
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

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public UpdateUserBioViewModel()
        {
            _userServices = UserServices.INSTANCE ?? throw new ArgumentNullException(nameof(UserServices.INSTANCE));
            _currentUser = UserController.INSTANCE.CurrentlyLoggedInUser ?? throw new InvalidOperationException("No user is currently logged in");

            Description = _currentUser.Description;

            SaveCommand = ReactiveCommand.Create(Save);
            CancelCommand = ReactiveCommand.Create(() => { });
        }

        private void Save()
        {
            try
            {
                _userServices.UpdateUserDescription(_currentUser, Description);
                Message = "Description updated successfully.";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
    }
}
