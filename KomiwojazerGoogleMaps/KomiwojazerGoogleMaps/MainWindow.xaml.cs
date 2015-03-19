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

namespace KomiwojazerGoogleMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Database.LinqDatabaseConnector databaseConnector;
        private Classes.UserLoginManager userLoginManager;
        private Windows.MainMenuWindow mainMenu;

        public MainWindow()
        {
            databaseConnector = new Database.LinqDatabaseConnector();
            InitializeComponent();
        }

        private void userLoginManager_loginSusses(object sender, Classes.LoginEventArgs e)
        {
            mainMenu = new Windows.MainMenuWindow(e.isAdmin);
            mainMenu.Show();
            mainMenu.showLoginWindow += mainMenu_showLoginWindow;

            this.Hide();
        }

        private void mainMenu_showLoginWindow(object sender, EventArgs e)
        {
            this.Show();
        }

        private void userLoginManager_loginFailed(object sender, Classes.LoginEventArgs e)
        {
            MessageBox.Show("Login failed");
        }

        private void buttonInfo_Click(object sender, RoutedEventArgs e)
        {
            Classes.InfoMessageProvider.showInfoMessage();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            userLoginManager = new Classes.UserLoginManager(textBoxUsername.Text,
                                                            textBoxPassword.Text,
                                                            checkBoxIsAdmin.IsChecked.Value);
            try
            {
                userLoginManager.loginFailed += userLoginManager_loginFailed;
                userLoginManager.loginSusses += userLoginManager_loginSusses;
                userLoginManager.logUserToMainMenu();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }                                        
    }
}
