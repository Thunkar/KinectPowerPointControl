using KinectPowerPointControl.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectPowerPointControl.View
{
    /// <summary>
    /// Lógica de interacción para MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        public MainView()
        {
            InitializeComponent();
            MainViewModel.Current.Initializer.Start();
        }

        private void OpenPPT_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Current.OpenPresentation();
        }

        private void ClosePPT_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Current.ClosePresentation();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Current.Browse();
        }


        private void AngleIncrease_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Current.KinectHandler.Tilt+=5;
        }

        private void AngleDecrease_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Current.KinectHandler.Tilt-=5;
        }
    }
}
