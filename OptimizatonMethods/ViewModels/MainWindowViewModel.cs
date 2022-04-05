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
        private IEnumerable<Method> _methods;
        private IEnumerable<Task> _tasks;
        #endregion

        #region Constructors
        public MainWindowViewModel(IUserRepository user, ITaskRepository task, IMethodRepository method)
        {
            _userRepository = user;
            _taskRepository = task;
            _methodRepository = method;
            _methods = _methodRepository.GetAllMethods();
            _tasks = _taskRepository.GetAllTasks();

        }
        #endregion

        #region Properties
        public IEnumerable<Method> Methods {
            get => _methods;
            set
            {
                _methods = _methodRepository.GetAllMethods();
                OnPropertyChanged();
            } 
        }
        #endregion

        #region Commands

        #endregion
    }
}
