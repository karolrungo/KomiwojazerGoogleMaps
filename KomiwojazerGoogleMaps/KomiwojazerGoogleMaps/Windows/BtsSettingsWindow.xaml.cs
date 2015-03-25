using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for BtsSettings.xaml
    /// </summary>
    public partial class BtsSettingsWindow : Window
    {
        private Database.LinqDatabaseConnector databaseConnector;

        public BtsSettingsWindow()
        {
            databaseConnector = new Database.LinqDatabaseConnector();
            InitializeComponent();
            refreshDatabaseBtsDataGrid();
        }

        private void buttonBTS_Search_Click(object sender, RoutedEventArgs e)
        {
            dataGridFoundBTS.ItemsSource = Classes.BtsFinder.findAddressess(textBoxCity.Text).DefaultView;
        }

        private void buttonDeleteBTS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTypes.BtsInfo btsInfo = getBtsInfoFromSelectedRowInDataGridBtsInDatabase();
                databaseConnector.deleteBTS(btsInfo.Location);
                refreshDatabaseBtsDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonAddBTS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTypes.BtsInfo btsInfo = getBtsInfoFromSelectedRowInDataGridLocations();
                databaseConnector.addBts(btsInfo.City,
                                         btsInfo.Location,
                                         btsInfo.Latitude,
                                         btsInfo.Longitude);
                refreshDatabaseBtsDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshDatabaseBtsDataGrid()
        {
            dataGridBTSidDatabase.ItemsSource = databaseConnector.selectAllBts();
        }

        private bool onlyOneRowSelected(DataGrid dataGrid)
        {
            return dataGrid.SelectedItems.Count == 1 ? true : false;
        }

        private DataTypes.BtsInfo getBtsInfoFromSelectedRowInDataGridLocations()
        {
            if (onlyOneRowSelected(dataGridFoundBTS))
            {
                DataRowView drv = (DataRowView)dataGridFoundBTS.SelectedItem;

                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                string location = (drv["Address"]).ToString();            
                float latitude = float.Parse((drv["Latitude"]).ToString());
                float longitude = float.Parse((drv["Longitude"]).ToString());

                return new DataTypes.BtsInfo(textBoxCity.Text,
                                             location,
                                             latitude,
                                             longitude);
            }
            else
            {
                throw new Exception("Only one row can be selected!");
            }
        }

        private DataTypes.BtsInfo getBtsInfoFromSelectedRowInDataGridBtsInDatabase()
        {
            if (onlyOneRowSelected(dataGridBTSidDatabase))
            {
                DataRowView drv = (DataRowView)dataGridBTSidDatabase.SelectedItem;

                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                string city = (drv["city"]).ToString();
                string location = (drv["cityGoogleString"]).ToString();
                float latitude = float.Parse((drv["latitude"]).ToString());
                float longitude = float.Parse((drv["longitude"]).ToString());

                return new DataTypes.BtsInfo(city,
                                             location,
                                             latitude,
                                             longitude);
            }
            else
            {
                throw new Exception("Only one row can be selected!");
            }
        }


    }
}
