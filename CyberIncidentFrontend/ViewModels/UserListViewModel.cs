using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CyberIncidentWPF.Helpers;
using CyberIncidentWPF.Models;
using CyberIncidentWPF.Services;

namespace CyberIncidentWPF.ViewModels
{
    public class UserListViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private ObservableCollection<User> _users;
        private string _newUsername = string.Empty;
        private string _newEmail = string.Empty;
        private string _newFullName = string.Empty;
        private string _newRole = "ANALYST";
        private bool _isLoading;
        private bool _isSubmitting;

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public string NewUsername
        {
            get => _newUsername;
            set => SetProperty(ref _newUsername, value);
        }

        public string NewEmail
        {
            get => _newEmail;
            set => SetProperty(ref _newEmail, value);
        }

        public string NewFullName
        {
            get => _newFullName;
            set => SetProperty(ref _newFullName, value);
        }

        public string NewRole
        {
            get => _newRole;
            set => SetProperty(ref _newRole, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsSubmitting
        {
            get => _isSubmitting;
            set => SetProperty(ref _isSubmitting, value);
        }

        public ObservableCollection<string> Roles { get; }

        public ICommand LoadUsersCommand { get; }
        public ICommand CreateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public UserListViewModel()
        {
            _apiService = ApiServiceProvider.Instance;
            _users = new ObservableCollection<User>();
            
            Roles = new ObservableCollection<string>
            {
                "ADMIN", "ANALYST", "VIEWER", "MANAGER"
            };

            LoadUsersCommand = new RelayCommand(async _ => await LoadUsersAsync());
            CreateUserCommand = new RelayCommand(async _ => await CreateUserAsync(), _ => CanCreateUser());
            DeleteUserCommand = new RelayCommand(async param => await DeleteUserAsync(param));

            // Initial load
            _ = LoadUsersAsync();
        }

        private async Task DeleteUserAsync(object param)
        {
            if (param is User user)
            {
                var result = MessageBox.Show($"Are you sure you want to delete user '{user.Username}'?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        IsLoading = true;
                        await _apiService.DeleteUserAsync(user.UserId);
                        Users.Remove(user);
                        MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                }
            }
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                IsLoading = true;
                var users = await _apiService.GetUsersAsync();
                Users = new ObservableCollection<User>(users);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CanCreateUser()
        {
            return !string.IsNullOrWhiteSpace(NewUsername) &&
                   !string.IsNullOrWhiteSpace(NewEmail) &&
                   !string.IsNullOrWhiteSpace(NewFullName) &&
                   !IsSubmitting;
        }

        private async Task CreateUserAsync()
        {
            try
            {
                IsSubmitting = true;
                var newUser = new User
                {
                    Username = NewUsername,
                    Email = NewEmail,
                    FullName = NewFullName,
                    Role = NewRole
                };

                await _apiService.CreateUserAsync(newUser);
                
                // Clear form
                NewUsername = string.Empty;
                NewEmail = string.Empty;
                NewFullName = string.Empty;
                NewRole = "ANALYST";

                MessageBox.Show("User created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Refresh list
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSubmitting = false;
            }
        }
    }
}
