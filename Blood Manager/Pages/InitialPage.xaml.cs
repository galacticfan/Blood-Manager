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

namespace Blood_Manager
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class InitialPage : Page
    {
        public InitialPage()
        {
            InitializeComponent();
        }

        // Exit and minimize button control
        private void exitApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void minimizeApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
        }

        // Mode select control
        private void localModeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/LocalModeMain.xaml", UriKind.Relative));
        }

        private void serverModeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/ServerModeMain.xaml", UriKind.Relative));
        }

        
    }
}
