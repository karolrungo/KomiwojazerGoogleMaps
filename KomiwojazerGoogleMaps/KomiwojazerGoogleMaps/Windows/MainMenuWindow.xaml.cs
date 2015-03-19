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

namespace KomiwojazerGoogleMaps.Windows
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public event EventHandler showLoginWindow;

        public MainMenuWindow()
        {
            InitializeComponent();
        }

        public MainMenuWindow(bool adminMode)
        {
            InitializeComponent();
            if (!adminMode)
            {
                buttonAddBTS.IsEnabled = false;
                buttonAddPeople.IsEnabled = false;
            }
        }

        private void buttonAddBTS_Click(object sender, RoutedEventArgs e)
        {
            Windows.BtsSettingsWindow btsSettingsWindow = new BtsSettingsWindow();
            btsSettingsWindow.Show();
        }

        private void buttonRoutePlanning_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonAddWorkers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            raiseShowLoginWindowEvent();
        }

        private void raiseShowLoginWindowEvent()
        {
            if (showLoginWindow != null)
            {
                showLoginWindow(this, EventArgs.Empty);
            }
        }
    }
}
