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

        public IEnumerable DataList
        {
            get => _dataList;
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        public List<Point3D> Point3D
        {
            get =>_point3D;
            set
            {
                _point3D = value;
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

                    var temp = new List<double>();

                    foreach (var item in points3D)
                    {
                        temp.Add(item.Z);
                    }

                    MessageBox.Show($"Минимальная себестоимость, у.е.: {temp.Min()}\n " +
                                    $"Температура в змеевике Т1, С: {points3D.Find(x => x.Z == temp.Min()).X}\n " +
                                    $"Температура в диффузоре Т2, С: {points3D.Find(x => x.Z == temp.Min()).Y}");

                    Point3D = points3D;
                });
            }
        }

        public RelayCommand TwoDChartCommand
        {
            get
            {
                return new RelayCommand(c =>
                {
                    double[] X = new double[Point3D.Count];
                    double[] Y = new double[Point3D.Count];
                    double[] Z = new double[Point3D.Count];
                    int i = 0;
                    foreach (var r in Point3D)
                    {
                        X[i] = r.X;
                        Y[i] = r.Y;
                        Z[i] = r.Z;
                        i++;
                    }
                   
                });
            }
        }

        public RelayCommand ThreeDChartCommand
        {
            get
            {
                return new RelayCommand(c =>
                {
                    
                    double[] X = new double[Point3D.Count];
                    double[] Y = new double[Point3D.Count];
                    double[] Z = new double[Point3D.Count];
                    int i = 0;
                    foreach (var r in Point3D)
                    {
                        X[i] = r.X;
                        Y[i] = r.Y;
                        Z[i] = r.Z;
                        i++;
                    }
                   

                });
            }
        }

        public RelayCommand AutorizationCommand
        {
            get
            {
                return new RelayCommand(c =>
                {
                    var autoriation = new AutorizationWindowViewModel(_userRepository);
                    ShowAutorization(autoriation);
                });
            }
        }

        #endregion
    }
}
