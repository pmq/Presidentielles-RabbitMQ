using System.Threading;
using System.Windows;
using TechDays.Backend.ViewModel;

namespace TechDays.Backend
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ZonesViewModel vm = new ZonesViewModel();
            DataContext = vm;
        }
        /// <summary>
        /// buttonStart_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            ZonesViewModel vm = DataContext as ZonesViewModel;
            if (vm != null)
                vm.Start();
        }

        /// <summary>
        /// buttonStop_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            ZonesViewModel vm = DataContext as ZonesViewModel;
            if (vm != null)
                vm.Stop();
            Thread.Sleep(100);
        }
        /// <summary>
        /// Window_Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, System.EventArgs e)
        {
            ZonesViewModel vm = DataContext as ZonesViewModel;
            if (vm != null)
                vm.Stop();
            Thread.Sleep(100);
        }
        /// <summary>
        /// buttonClosed_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClosed_Click(object sender, RoutedEventArgs e)
        {
            ZonesViewModel vm = DataContext as ZonesViewModel;
            if (vm != null)
                vm.Closed();
        }
        /// <summary>
        /// buttonInit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInit_Click(object sender, RoutedEventArgs e)
        {
            ZonesViewModel vm = DataContext as ZonesViewModel;
            if (vm != null)
                vm.Raz();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAll_Click(object sender, RoutedEventArgs e)
        {
            ZonesViewModel vm = DataContext as ZonesViewModel;
            if (vm != null)
                vm.All();
        }
    }
}
