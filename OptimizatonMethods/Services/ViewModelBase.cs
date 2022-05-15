﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OptimizatonMethods.Services
{
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged, IDisposable
    {

        public void Dispose()
        {
            OnDispose();
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        protected virtual void OnDispose()
        {
        }

        ///
        /// Окно в котором показывается текущий ViewModel
        ///
       
        private AutorizationWindow _autorizationWindow = null;

        private AdminWindow _adminWindow = null;

        private AddUserWindow _addUserWindow = null;

        private AddMethodWindow _addMethodWindow = null;

        private AddTaskWindow _addTaskWindow = null;

        private GeneticAlgSettingWindow _algSettingWindow = null;

        protected virtual void Closed()
        {

        }

        ///
        /// Методы вызываемый для закрытия окна связанного с ViewModel
        ///

        public bool CloseAutorizationWindow()
        {
            var result = false;
            if (_autorizationWindow != null)
            {
                _autorizationWindow.Close();
                _autorizationWindow = null;
                result = true;
            }

            return result;
        }

        public bool CloseAddUserWindow()
        {

            var result = false;
            if (_addUserWindow != null)
            {
                _addUserWindow.Close();
                _addUserWindow = null;
                result = true;
            }
            return result;
        }
        public bool CloseAddMethodWindow()
        {

            var result = false;
            if (_addMethodWindow != null)
            {
                _addMethodWindow.Close();
                _addMethodWindow = null;
                result = true;
            }
            return result;
        }

        public bool CloseAddTaskWindow()
        {

            var result = false;
            if (_addTaskWindow != null)
            {
                _addTaskWindow.Close();
                _addTaskWindow = null;
                result = true;
            }
            return result;
        }
        public bool CloseGeneticAlgSettingWindow()
        {

            var result = false;
            if (_algSettingWindow != null)
            {
                _algSettingWindow.Close();
                _algSettingWindow = null;
                result = true;
            }
            return result;
        }

        ///
        /// Метод показа ViewModel в окне
        ///
        /// viewModel">
        protected void ShowAutorization(ViewModelBase viewModel)
        {
            viewModel._autorizationWindow = new AutorizationWindow
            {
                DataContext = viewModel
            };
            viewModel._autorizationWindow.Closed += (sender, e) => Closed();
            viewModel._autorizationWindow.Show();

        }

        protected void ShowAdmin(ViewModelBase viewModel)
        {
            viewModel._adminWindow = new AdminWindow
            {
                DataContext = viewModel
            };
            viewModel._adminWindow.Closed += (sender, e) => Closed();
            viewModel._adminWindow.Show();

        }
        protected void ShowAddUser(ViewModelBase viewModel, string title)
        {
            viewModel._addUserWindow = new AddUserWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._addUserWindow.Closed += (sender, e) => Closed();
            viewModel._addUserWindow.Show();
            
        }
        protected void ShowAddMethod(ViewModelBase viewModel, string title)
        {
            viewModel._addMethodWindow = new AddMethodWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._addMethodWindow.Closed += (sender, e) => Closed();
            viewModel._addMethodWindow.Show();

        }
        protected void ShowAddTask(ViewModelBase viewModel, string title)
        {
            viewModel._addTaskWindow = new AddTaskWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._addTaskWindow.Closed += (sender, e) => Closed();
            viewModel._addTaskWindow.Show();

        }
        protected void ShowGeneticAlgSettingWindow(ViewModelBase viewModel, string title)
        {
            viewModel._algSettingWindow = new GeneticAlgSettingWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._algSettingWindow.Closed += (sender, e) => Closed();
            viewModel._algSettingWindow.Show();

        }

    }
}
