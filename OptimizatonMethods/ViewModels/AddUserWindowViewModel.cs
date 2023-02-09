using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OptimizatonMethods.Models.Data.Abstract;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;

namespace OptimizatonMethods.ViewModels
{
    public class AddUserWindowViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        private readonly User? _user;
        private readonly AdminWindowViewModel _viewModelBase;
        private string _username;
        private string _password;
        
        public AddUserWindowViewModel(IUserRepository userRepository, User? user, AdminWindowViewModel viewModelBase)
        {
            _userRepository = userRepository;
            _user = user;
            _viewModelBase = viewModelBase;

            if (user != null)
            {
                Username = user.Username;
                Password = user.Password;
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddOrUpdateUserCommand
        {
            get
            {
                return new RelayCommand(async x =>
                {
                    if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                    {
                        MessageBox.Show("Вы не указали логин и/или пароль пользователя", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    if (_user != null)
                    {
                        _user.Username = Username;
                        _user.Password = Password;
                        await _userRepository.SaveUserAsync(_user);
                        MessageBox.Show("Информация о пользователе обновлена", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        var newUser = new User()
                        {
                            Username = Username,
                            Password = Password
                        };
                        await _userRepository.SaveUserAsync(newUser);
                        MessageBox.Show("Пользователь успешно добавлен", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }

                    _viewModelBase.UpdateUsersProp();
                    CloseAddUserWindow();

                });
            }
        }
    }
}
