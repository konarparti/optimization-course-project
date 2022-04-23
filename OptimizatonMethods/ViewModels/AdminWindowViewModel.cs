using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizatonMethods.Models.Data.Abstract;
using OptimizatonMethods.Services;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;

namespace OptimizatonMethods.ViewModels
{
    public class AdminWindowViewModel : ViewModelBase
    {
        private readonly IMethodRepository _method;
        private readonly ITaskRepository _task;
        private readonly IUserRepository _user;
        private IEnumerable<Method> _methods;
        private IEnumerable<Task> _tasks;
        private IEnumerable<User> _users;

        public AdminWindowViewModel(IMethodRepository method, ITaskRepository task, IUserRepository user)
        {
            _method = method;
            _task = task;
            _user = user;

            _methods = _method.GetAllMethods();
            _tasks = _task.GetAllTasks();
            _users = _user.GetAllUsers();
        }

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


    }
}
