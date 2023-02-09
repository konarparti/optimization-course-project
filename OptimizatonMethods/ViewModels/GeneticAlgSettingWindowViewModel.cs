using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OptimizatonMethods.Models;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;

namespace OptimizatonMethods.ViewModels;

public class GeneticAlgSettingWindowViewModel : ViewModelBase
{
    #region Variables

    private int _countPopulation;
    private readonly Task _task;
    private readonly MainWindowViewModel _viewModelBase;

    #endregion

    #region Constructors

    public GeneticAlgSettingWindowViewModel(Task task, MainWindowViewModel viewModelBase)
    {
        _task = task;
        _viewModelBase = viewModelBase;
    }

    #endregion

    #region Properties
    public int CountPopulation
    {
        get => _countPopulation;
        set
        {
            _countPopulation = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public RelayCommand CalcCommand
    {
        get
        {
            return new RelayCommand(command =>
            {
                if (CountPopulation > 0)
                {
                    var calc = new MathModel(_task);
                    CloseGeneticAlgSettingWindow();
                    calc.GeneticAlg(CountPopulation, out var points);
                    _viewModelBase.SetGeneticAlgData(points);
                }
                else
                {
                    MessageBox.Show("Введите натурально число поколений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }

    #endregion

    #region Functions



    #endregion
}