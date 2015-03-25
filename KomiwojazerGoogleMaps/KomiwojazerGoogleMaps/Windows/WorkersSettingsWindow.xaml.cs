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
    /// Interaction logic for WorkersSettings.xaml
    /// </summary>
    public partial class WorkersSettingsWindow : Window
    {
        Database.LinqDatabaseConnector databaseConnector;

        public WorkersSettingsWindow()
        {
            databaseConnector = new Database.LinqDatabaseConnector();
            InitializeComponent();
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login  = getUserLoginFromSelectedRowInDataGrid();
                databaseConnector.deleteUser(login);
                refreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Classes.UserLoginManager userDataValidater = new Classes.UserLoginManager(textBoxLogin.Text,
                                                                                          textBoxPassword.Text,
                                                                                          checkBoxIsAdmin.IsChecked.Value);
                userDataValidater.validateUserData();

                databaseConnector.addUser(textBoxLogin.Text,
                                          textBoxPassword.Text,
                                          checkBoxIsAdmin.IsChecked.Value);
                refreshDataGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshDataGrid();
        }

        private void refreshDataGrid()
        {
            dataGridUsersInDatabase.ItemsSource = databaseConnector.selectAllUsers();
        }

        private string getUserLoginFromSelectedRowInDataGrid()
        {
            if (onlyOneRowSelected(dataGridUsersInDatabase))
            {
                Database.People drv = (Database.People)dataGridUsersInDatabase.SelectedItem;
                return drv.username;
            }
            else
            {
                throw new Exception("Only one row can be selected!");
            }
        }

        private bool onlyOneRowSelected(DataGrid dataGrid)
        {
            return dataGrid.SelectedItems.Count == 1 ? true : false;
        }

    }
}
