using Autofac;
using OptimizatonMethods.Models.Data.Abstract;
using OptimizatonMethods.Models.Data.EntityFramework;
using OptimizatonMethods.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OptimizatonMethods
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EFUserRepository>().As<IUserRepository>();
            builder.RegisterType<EFMethodRepository>().As<IMethodRepository>();
            builder.RegisterType<EFTaskRepository>().As<ITaskRepository>();
            builder.RegisterType<MO_courseContext>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            var container = builder.Build();
            var mainWindowViewModel = container.Resolve<MainWindowViewModel>();
            var mainWindow = new MainWindow { DataContext = mainWindowViewModel };
            mainWindow.Show();
        }
    }
}
