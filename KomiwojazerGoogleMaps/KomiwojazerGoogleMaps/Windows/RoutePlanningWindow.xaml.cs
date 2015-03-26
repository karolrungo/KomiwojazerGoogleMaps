using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
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
    /// Interaction logic for RoutePlanningWindow.xaml
    /// </summary>
    /// 
    public partial class RoutePlanningWindow : Window
    {
        private List<Database.Bt> btsList;
        private Database.LinqDatabaseConnector databaseConnection;

        public RoutePlanningWindow()
        {
            InitializeComponent();

            btsList = new List<Database.Bt>();
            databaseConnection = new Database.LinqDatabaseConnector();

            gmap.MapProvider = GMapProviders.GoogleMap;
            gmap.Position = getCoordinatesOfLocation("Piątek, Polska"); // geograficzny srodek Polski ;)
            gmap.MinZoom = 0;
            gmap.MaxZoom = 24;
            gmap.Zoom = 6;
        }

        private PointLatLng getCoordinatesOfLocation(string location)
        {
            float longitude, latitude;

            DataTable dt = Classes.BtsFinder.findAddressess(location);
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            longitude = float.Parse(dt.Rows[0]["Longitude"].ToString());
            latitude = float.Parse(dt.Rows[0]["Latitude"].ToString());

            return new PointLatLng(latitude, longitude);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridBts.ItemsSource = databaseConnection.selectAllBts();
        }

        private void btnStartPlanning_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonDeleteBtsFromList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonAddBtsToList_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
