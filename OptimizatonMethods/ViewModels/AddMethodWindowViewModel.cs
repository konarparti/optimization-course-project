using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OptimizatonMethods.Models.Data.Abstract;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;

namespace OptimizatonMethods.ViewModels
{
    public class AddMethodWindowViewModel : ViewModelBase
    {
        private readonly IMethodRepository _methodRepository;
        private readonly Method? _method;
        private readonly AdminWindowViewModel _viewModelBase;
        private string _name;
        private bool _active;

        public AddMethodWindowViewModel(IMethodRepository methodRepository, Method? method, AdminWindowViewModel viewModelBase)
        {
            _methodRepository = methodRepository;
            _method = method;
            _viewModelBase = viewModelBase;

            if (method != null)
            {
                Name = method.Name;
                Active = Convert.ToBoolean(method.Activated);
            }
        }

        public string Name 
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddOrUpdateMethodCommand
        {
            get
            {
                return new RelayCommand(x =>
                {
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        MessageBox.Show("Вы не указали название метода", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    if (_method != null)
                    {
                        _method.Name = Name;
                        _method.Activated = Active.ToString();
                        _methodRepository.SaveMethod(_method);
                        MessageBox.Show("Информация о методе обновлена", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        var newMethod = new Method()
                        {
                            Name = Name,
                            Activated = Active.ToString()
                        };
                        _methodRepository.SaveMethod(newMethod);
                        MessageBox.Show("Метод успешно добавлен", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }

                    _viewModelBase.UpdateMethodsProp();
                    CloseAddMethodWindow();

                });
            }
        }
    }
}
