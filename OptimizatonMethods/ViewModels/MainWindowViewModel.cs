using OptimizatonMethods.Models;
using OptimizatonMethods.Models.Data.Abstract;
using System.Collections;
using System.Collections.Generic;
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
        private RelayCommand? _calculateCommand;
        private IEnumerable _dataList;
        #endregion

        #region Constructors
        public MainWindowViewModel(IUserRepository user, ITaskRepository task, IMethodRepository method)
        {
            _userRepository = user;
            _taskRepository = task;
            _methodRepository = method;
            _allMethods = _methodRepository.GetAllMethods();
            _allTasks = _taskRepository.GetAllTasks();

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

        public IEnumerable DataList
        {
            get => _dataList;
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command

        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(c =>
                {
                    var calc = new MathModel();
                    calc.Calculate(out var points2D, out var points3D);
                    DataList = points3D;
                });
            }
        }
        #endregion
    }
}
