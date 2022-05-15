using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizatonMethods.Models;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;

namespace OptimizatonMethods.ViewModels;

public class GeneticAlgSettingWindowViewModel : ViewModelBase
{
    private readonly Task _task;

    #region Variables
    
    private int _countPopulation;

    #endregion

    #region Constructors

    public GeneticAlgSettingWindowViewModel(Task task)
    {
        _task = task;
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
                var calc = new MathModel(_task);
                CloseGeneticAlgSettingWindow();
                calc.GeneticAlg(CountPopulation);
            });
        }
    }

    #endregion

    #region Functions



    #endregion
}