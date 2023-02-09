using System;
using OptimizatonMethods.Models;
using OptimizatonMethods.Models.Data.Abstract;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OptimizatonMethods.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
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
        private Task? _selectedTask;
        private Method? _selectedMethod;
        private RelayCommand? _calculateCommand;
        private IEnumerable _dataList;
        private List<Point3D> _point3D = new();
        private PlotModel _myModel;

        #endregion

        #region Constructors
        public MainWindowViewModel(IUserRepository user, ITaskRepository task, IMethodRepository method)
        {
            _userRepository = user;
            _taskRepository = task;
            _methodRepository = method;
            _allMethods = _methodRepository.GetAllMethods().Where(m => m.Activated?.ToLower() == "true");
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

        public Task? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                TaskChanged();
                OnPropertyChanged();
            }
        }

        public Method? SelectedMethod
        {
            get => _selectedMethod;
            set
            {
                _selectedMethod = value;
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

        public PlotModel MyModel
        {
            get => _myModel;
            set
            {
                _myModel = value;
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
                    CheckValues(out var errors);
                    if (errors != string.Empty)
                    {
                        MessageBox.Show(errors, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var calc = new MathModel(Task);

                    if (SelectedMethod.Name == "Генетический алгоритм")
                    {
                        var genAlg = new GeneticAlgSettingWindowViewModel(Task, this);
                        ShowGeneticAlgSettingWindow(genAlg, "Настройка генетического алгоритма");
                    }

                    if (SelectedMethod.Name == "Метод сканирования")
                    {
                        calc.Calculate(out var points3D);
                        DataList = points3D;

                        var temp = new List<double>();

                        foreach (var item in points3D)
                        {
                            temp.Add(item.Z);
                        }

                        _myModel = new PlotModel { Title = "S = F(T1, T2)", TitleFontSize = 16 };
                        Func<double, double, double> peaks = (x, y) => calc.Function(x, y) > 1000 ? 1000 : calc.Function(x, y);

                        double x0 = (double)Task.T1min;
                        double x1 = (double)Task.T1max;
                        double y0 = (double)Task.T2min;
                        double y1 = (double)Task.T2max;

                        var xx = ArrayBuilder.CreateVector(x0, x1, 100);
                        var yy = ArrayBuilder.CreateVector(y0, y1, 100);
                        var peaksData = ArrayBuilder.Evaluate(peaks, xx, yy);

                        var cs = new ContourSeries
                        {
                            Color = OxyColors.Black,
                            LabelBackground = OxyColors.White,
                            ColumnCoordinates = yy,
                            RowCoordinates = xx,
                            Data = peaksData,
                            TrackerFormatString = "T1 = {2:0.00}, T2 = {4:0.00}" + Environment.NewLine + "S = {6:0.00}"
                        };

                        _myModel.Series.Add(cs);

                        _myModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Температура на змеевике, С" });
                        _myModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Температура на диффузоре, С" });

                        MyModel = _myModel;

                        MessageBox.Show($"Минимальная себестоимость, у.е.: {temp.Min()}\n " +
                                        $"Температура в змеевике Т1, С: {points3D.Find(x => x.Z == temp.Min()).X}\n " +
                                        $"Температура в диффузоре Т2, С: {points3D.Find(x => x.Z == temp.Min()).Y}");
                    }

                    if (SelectedMethod.Name == "Метод Бокса")
                    {
                        calc.Calc(out var points3D);
                        DataList = points3D;

                        var temp = new List<double>();

                        foreach (var item in points3D)
                        {
                            temp.Add(item.Z);
                        }

                        MessageBox.Show($"Минимальная себестоимость, у.е.: {temp.Min()}\n " +
                                        $"Температура в змеевике Т1, С: {points3D.Find(x => x.Z == temp.Min()).X}\n " +
                                        $"Температура в диффузоре Т2, С: {points3D.Find(x => x.Z == temp.Min()).Y}");
                    }

                });
            }
        }

        public RelayCommand TwoDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart2DWindow(DataList as List<Point3D>, Task);
                    test.Show();
                });


            }
        }

        public RelayCommand ThreeDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart3DWindow(DataList as List<Point3D>, Task);
                    test.Show();
                });
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

        private void CheckValues(out string errors)
        {
            errors = string.Empty;
            if (SelectedMethod == null)
                errors += "Вы не выбрали метод\n";
            if (SelectedTask == null)
                errors += "Вы не выбрали задание\n";
        }

        private void TaskChanged()
        {
            Task = AllTasks.First(x => x.Name == _selectedTask.Name);
        }

        public void UpdateMethod()
        {
            AllMethods = _methodRepository.GetAllMethods().Where(m => m.Activated?.ToLower() == "true");
        }
        public void UpdateTask()
        {
            AllTasks = _taskRepository.GetAllTasks();
        }

        public void SetGeneticAlgData(List<Point3D> points)
        {
            DataList = points;
            var temp = new List<double>();

            foreach (var item in points)
            {
                temp.Add(item.Z);
            }

            MessageBox.Show($"Минимальная себестоимость, у.е.: {temp.Min()}\n " +
                            $"Температура в змеевике Т1, С: {points.Find(x => x.Z == temp.Min()).X}\n " +
                            $"Температура в диффузоре Т2, С: {points.Find(x => x.Z == temp.Min()).Y}");
        }
    }
}
