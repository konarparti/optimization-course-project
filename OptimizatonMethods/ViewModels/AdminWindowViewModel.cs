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

        private readonly IMethodRepository _method;
        private readonly ITaskRepository _task;
        private readonly IUserRepository _user;
        private IEnumerable<Method> _methods;
        private IEnumerable<Task> _tasks;
        private IEnumerable<User> _users;
        private Task _selectedTask;
        private Method _selectedMethod;
        private User _selectedUser;

        #endregion

        #region Constructors
        public AdminWindowViewModel(IMethodRepository method, ITaskRepository task, IUserRepository user)
        {
            _method = method;
            _task = task;
            _user = user;

            _methods = _method.GetAllMethods();
            _tasks = _task.GetAllTasks();
            _users = _user.GetAllUsers();
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
                        MessageBox.Show($"Действительно удалить {_selectedTask.Name}?", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                
                });
            }
        }

        public RelayCommand AddMethod
        {
            get
            {
                return new RelayCommand(c =>
                {

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
                        MessageBox.Show($"{_selectedMethod.Id}\n {_selectedMethod.Name}", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
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
                        MessageBox.Show($"Действительно удалить {_selectedMethod.Name}?", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                });
            }
        }


        public RelayCommand AddUser
        {
            get
            {
                return new RelayCommand(c =>
                {

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
                        MessageBox.Show($"{_selectedUser.Id}\n {_selectedUser.Username}", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
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
                        MessageBox.Show($"Действительно удалить {_selectedUser.Username}?", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                });
            }
        }
        #endregion

    }
}
