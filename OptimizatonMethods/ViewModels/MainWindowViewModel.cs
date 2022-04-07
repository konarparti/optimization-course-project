using OptimizatonMethods.Models.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_Classes;

namespace OptimizatonMethods.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMethodRepository _methodRepository;
        private IEnumerable<Method> _allMethods;
        private IEnumerable<Task> _allTasks;
        private Task _task;
        #endregion

        #region Constructors
        public MainWindowViewModel(IUserRepository user, ITaskRepository task, IMethodRepository method)
        {
            _userRepository = user;
            _taskRepository = task;
            _methodRepository = method;
            _allMethods = _methodRepository.GetAllMethods();
            _allTasks = _taskRepository.GetAllTasks();
            _task = _taskRepository.GetTask(1);

        }
        #endregion

        #region Properties
        public IEnumerable<Method> AllMethods {
            get => _allMethods;
            set
            {
                _allMethods = value;
                OnPropertyChanged();
            } 
        }

        public IEnumerable<Task> AllTasks { 
            get => _allTasks;
            set
            {
                _allTasks = value;
                OnPropertyChanged();
            }
        }

        public Task Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        #endregion
    }
}
