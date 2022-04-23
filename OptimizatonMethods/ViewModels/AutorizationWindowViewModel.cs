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
        private readonly IUserRepository _userRepository;
        private readonly IMethodRepository _methodRepository;
        private readonly ITaskRepository _taskRepository;
        private string _username;
        private string _password;

        public AutorizationWindowViewModel(IUserRepository userRepository, IMethodRepository methodRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _methodRepository = methodRepository;
            _taskRepository = taskRepository;
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
                    if (_userRepository.VerifyUser(Username, Password))
                    {
                        var adminPanel = new AdminWindowViewModel(_methodRepository, _taskRepository, _userRepository);
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
