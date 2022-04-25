using System;
using OptimizatonMethods.Models;
using OptimizatonMethods.Models.Data.Abstract;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OptimizatonMethods.Services;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;

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
        private Task _selectedTask;
        private RelayCommand? _calculateCommand;
        private IEnumerable _dataList;
        private List<Point3D> _point3D = new();
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
        public IEnumerable<Method> AllMethods
        {
            get => _allMethods;
            set
            {
                _allMethods = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Task> AllTasks
        {
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

        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                TaskChanged();
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
                    var calc = new MathModel(Task);
                    calc.Calculate(out var points2D, out var points3D);
                    DataList = points3D;

                    var temp = new List<double>();

                    foreach (var item in points3D)
                    {
                        temp.Add(item.Z);
                    }

                    MessageBox.Show($"Минимальная себестоимость, у.е.: {temp.Min()}\n " +
                                    $"Температура в змеевике Т1, С: {points3D.Find(x => x.Z == temp.Min()).X}\n " +
                                    $"Температура в диффузоре Т2, С: {points3D.Find(x => x.Z == temp.Min()).Y}");

                });
            }
        }

        public RelayCommand TwoDChartCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public RelayCommand ThreeDChartCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public RelayCommand AutorizationCommand
        {
            get
            {
                return new RelayCommand(c =>
                {
                    var autoriation = new AutorizationWindowViewModel(_userRepository, _methodRepository, _taskRepository, this);
                    ShowAutorization(autoriation);
                });
            }
        }

        #endregion

        private void TaskChanged()
        {
            Task = AllTasks.First(x => x.Name == _selectedTask.Name);
        }

        public void UpdateMethod()
        {
            AllMethods = _methodRepository.GetAllMethods();
        }
    }
}
