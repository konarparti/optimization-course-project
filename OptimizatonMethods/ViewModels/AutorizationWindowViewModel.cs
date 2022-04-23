using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OptimizatonMethods.Models.Data.Abstract;
using OptimizatonMethods.Services;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;


namespace OptimizatonMethods.ViewModels
{
    public class AutorizationWindowViewModel : ViewModelBase
    {
        private readonly IUserRepository _user;
        private string _username;
        private string _password;

        public AutorizationWindowViewModel(IUserRepository user)
        {
            _user = user;
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

        public RelayCommand LoginCommand
        {
            get
            {
                return new RelayCommand(c =>
                {
                    if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                    {
                        MessageBox.Show("Вы не указали логин или пароль", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }
                    if (_user.VerifyUser(Username, Password))
                    {
                        var adminPanel = new AdminWindowViewModel();
                        ShowAdmin(adminPanel);
                        CloseAutorizationWindow();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                });
            }
        }
    }
}
