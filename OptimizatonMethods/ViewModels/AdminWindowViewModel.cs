using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OptimizatonMethods.Models.Data.Abstract;
using OptimizatonMethods.Services;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;

namespace OptimizatonMethods.ViewModels
{
    public class AdminWindowViewModel : ViewModelBase
    {
        #region Variables

        private readonly IMethodRepository _methodRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly MainWindowViewModel _viewModelBase;
        private IEnumerable<Method> _methods;
        private IEnumerable<Task> _tasks;
        private IEnumerable<User> _users;
        private Task _selectedTask;
        private Method _selectedMethod;
        private User _selectedUser;

        #endregion

        #region Constructors
        public AdminWindowViewModel(IMethodRepository methodRepository, ITaskRepository task, IUserRepository user, MainWindowViewModel viewModelBase)
        {
            _methodRepository = methodRepository;
            _taskRepository = task;
            _userRepository = user;
            _viewModelBase = viewModelBase;

            _methods = _methodRepository.GetAllMethods();
            _tasks = _taskRepository.GetAllTasks();
            _users = _userRepository.GetAllUsers();
        }

        #endregion


        #region Properties
        public IEnumerable<Method> Methods
        {
            get => _methods;
            set
            {
                _methods = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }

        public Method SelectedMethod
        {
            get => _selectedMethod;
            set
            {
                _selectedMethod = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand AddTask
        {
            get
            {
                return new RelayCommand(c =>
                {
                    
                });
            }
        }

        public RelayCommand UpdateTask
        {
            get
            {
                return new RelayCommand(c =>
                {
                    if (_selectedTask == null)
                    {
                        MessageBox.Show("Выберите задание, которое необходимо изменить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"{_selectedTask.Id}\n {_selectedTask.Name}", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }

        public RelayCommand DeleteTask
        {
            get
            {
                return new RelayCommand(c =>
                {
                    if (_selectedTask == null)
                    {
                        MessageBox.Show("Выберите задание, которое необходимо удалить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (MessageBox.Show($"Вы уверенны что хотите удалить {_selectedTask.Name} из библиотеки заданий?", "Информация",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            _taskRepository.DeleteTask((int)_selectedTask.Id);
                        }
                    }
                
                    UpdateTasksProp();
                });
            }
        }

        public RelayCommand AddMethod
        {
            get
            {
                return new RelayCommand(c =>
                {
                    var addMethod = new AddMethodWindowViewModel(_methodRepository, null, this);
                    ShowAddMethod(addMethod, "Добавление метода");
                });
            }
        }

        public RelayCommand UpdateMethod
        {
            get
            {
                return new RelayCommand(c =>
                {
                    if (_selectedMethod == null)
                    {
                        MessageBox.Show("Выберите метод, который необходимо изменить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var addMethod = new AddMethodWindowViewModel(_methodRepository, _selectedMethod, this);
                        ShowAddMethod(addMethod, "Изменение метода");
                    }
                });
            }
        }

        public RelayCommand DeleteMethod
        {
            get
            {
                return new RelayCommand(c =>
                {
                    if (_selectedMethod == null)
                    {
                        MessageBox.Show("Выберите метод, который необходимо удалить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (MessageBox.Show($"Вы уверены что хотите удалить {_selectedMethod.Name} из библиотеки методов?", "Информация",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            _methodRepository.DeleteMethod((int)_selectedMethod.Id);
                        }
                    }

                    UpdateMethodsProp();
                });
            }
        }


        public RelayCommand AddUser
        {
            get
            {
                return new RelayCommand(  c =>
                {
                    var addUser = new AddUserWindowViewModel(_userRepository, null, this);
                    ShowAddUser(addUser, "Добавление пользователя");
                });
            }
        }

        public RelayCommand UpdateUser
        {
            get
            {
                return new RelayCommand(c =>
                {
                    if (_selectedUser == null)
                    {
                        MessageBox.Show("Выберите пользователя, которого необходимо изменить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var addUser = new AddUserWindowViewModel(_userRepository, _selectedUser, this);
                        ShowAddUser(addUser, "Изменение пользователя");
                    }
                });
            }
        }

        public RelayCommand DeleteUser
        {
            get
            {
                return new RelayCommand(c =>
                {
                    if (_selectedUser == null)
                    {
                        MessageBox.Show("Выберите пользователя, которого необходимо удалить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (MessageBox.Show(
                                $"Вы уверены что хотите удалить пользователя {_selectedUser.Username} из библиотеки пользователей?",
                                "Информация",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            _userRepository.DeleteUser(_selectedUser.Id);
                        }
                    }

                    UpdateUsersProp();

                });
            }
        }
        #endregion

        #region Functions

        public void UpdateUsersProp()
        {
            Users = _userRepository.GetAllUsers();
        }
        public void UpdateMethodsProp()
        {
            Methods = _methodRepository.GetAllMethods();
            _viewModelBase.UpdateMethod();
        }
        public void UpdateTasksProp()
        {
            Tasks = _taskRepository.GetAllTasks();
        }

        #endregion
    }
}
