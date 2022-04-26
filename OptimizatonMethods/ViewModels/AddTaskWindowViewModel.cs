using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OptimizatonMethods.Models.Data.Abstract;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;
using WPF_MVVM_Classes;

namespace OptimizatonMethods.ViewModels
{
    public class AddTaskWindowViewModel : ViewModelBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly Task? _task;
        private readonly AdminWindowViewModel _viewModelBase;
        private Task _newTask = new Task();
        public AddTaskWindowViewModel(ITaskRepository taskRepository, Task? task, AdminWindowViewModel viewModelBase)
        {
            _taskRepository = taskRepository;
            _task = task;
            _viewModelBase = viewModelBase;

            if (task != null)
            {
                NewTask.Name = task.Name;
                NewTask.Alpha = task.Alpha;
                NewTask.Beta = task.Beta;
                NewTask.Delta = task.Delta;
                NewTask.Mu = task.Mu;
                NewTask.G = task.G;
                NewTask.A = task.A;
                NewTask.N = task.N;
                NewTask.T1min = task.T1min;
                NewTask.T1max = task.T1max;
                NewTask.T2min = task.T2min;
                NewTask.T2max = task.T2max;
                NewTask.DifferenceTemp = task.DifferenceTemp;
                NewTask.Price = task.Price;
                NewTask.Step = task.Step;
            }
        }

        public Task NewTask
        {
            get => _newTask;
            set
            {
                _newTask = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddOrUpdateTaskCommand
        {
            get
            {
                return new RelayCommand(x =>
                {
                    if (string.IsNullOrWhiteSpace(NewTask.Name))
                    {
                        MessageBox.Show("Вы не указали название метода", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    if (_task != null)
                    {
                        _task.Name = NewTask.Name;
                        _task.Alpha = NewTask.Alpha;
                        _task.Beta = NewTask.Beta;
                        _task.Delta = NewTask.Delta;
                        _task.Mu = NewTask.Mu;
                        _task.G = NewTask.G;
                        _task.A = NewTask.A;
                        _task.N = NewTask.N;
                        _task.T1min = NewTask.T1min;
                        _task.T1max = NewTask.T1max;
                        _task.T2min = NewTask.T2min;
                        _task.T2max = NewTask.T2max;
                        _task.DifferenceTemp = NewTask.DifferenceTemp;
                        _task.Price = NewTask.Price;
                        _task.Step = NewTask.Step;

                        _taskRepository.SaveTask(_task);
                        MessageBox.Show("Информация о задании обновлена", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        var task = new Task()
                        {
                            Name = NewTask.Name,
                            Alpha = NewTask.Alpha,
                            Beta = NewTask.Beta,
                            Delta = NewTask.Delta,
                            Mu = NewTask.Mu,
                            G = NewTask.G,
                            A = NewTask.A,
                            N = NewTask.N,
                            T1min = NewTask.T1min,
                            T1max = NewTask.T1max,
                            T2min = NewTask.T2min,
                            T2max = NewTask.T2max,
                            DifferenceTemp = NewTask.DifferenceTemp,
                            Price = NewTask.Price,
                            Step = NewTask.Step
                        };
                        _taskRepository.SaveTask(task);
                        MessageBox.Show("Задание успешно добавлено", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }

                    _viewModelBase.UpdateTasksProp();
                    CloseAddTaskWindow();

                });
            }
        }
    }
}
