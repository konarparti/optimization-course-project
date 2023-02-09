using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OptimizatonMethods
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        public AutorizationWindow()
        {
            InitializeComponent();
        }

        private void AutorizationWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            FontSize = (ActualHeight + ActualHeight / ActualWidth * ActualWidth) / 44;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
