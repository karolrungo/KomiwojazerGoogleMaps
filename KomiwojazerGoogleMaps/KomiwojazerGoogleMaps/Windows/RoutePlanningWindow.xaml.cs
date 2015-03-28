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
        private HashSet<Database.Bt> btsList;
        List<int> visitOrder;
        double visitOrderDistance = 0.0;
        private Database.LinqDatabaseConnector databaseConnection;

        public RoutePlanningWindow()
        {
            InitializeComponent();

            btsList = new HashSet<Database.Bt>();
            databaseConnection = new Database.LinqDatabaseConnector();

            dataGridBts.IsReadOnly = true;
            dataGridList.IsReadOnly = true;
        }

        private PointLatLng getCoordinatesOfLocation(string location)
        {
            float longitude, latitude;

            DataTable dt = Classes.BtsFinder.findLocations(location);
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            longitude = float.Parse(dt.Rows[0]["Longitude"].ToString());
            latitude = float.Parse(dt.Rows[0]["Latitude"].ToString());

            return new PointLatLng(latitude, longitude);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridBts.ItemsSource = databaseConnection.selectAllBts();

            gmap.MapProvider = GMapProviders.GoogleMap;
            gmap.Position = getCoordinatesOfLocation("Piątek, Polska"); // geograficzny srodek Polski ;)
            gmap.MinZoom = 0;
            gmap.MaxZoom = 24;
            gmap.Zoom = 6;
        }

        private void btnStartPlanning_Click(object sender, RoutedEventArgs e)
        {
            //Algorithm.TravelingSalesmanProblemSolver solver = new Algorithm.TravelingSalesmanProblemSolver(btsList);
            try
            {
                //solver.Start();
                //visitOrder = solver.getOptimalOrder();
                var algorithmResult = Algorithm.TravelingSalesmanProblemSolver.solve(btsList.ToList());
                if (Algorithm.TravelingSalesmanProblemSolver.Result.Success == algorithmResult)
                {
                    visitOrder = Algorithm.TravelingSalesmanProblemSolver.getOptimalRoute();
                    visitOrderDistance = Algorithm.TravelingSalesmanProblemSolver.getOptimalRouteDistance();
                    drawOptimalRouteOnMap();
                    MessageBox.Show(generateOptimalListString());
                }
                else
                {
                    var errorCode = (Algorithm.TravelingSalesmanProblemSolver.Result.GoogleConnectionError == algorithmResult) ? "Google Connection" : "Not Found Hamiltonian Cycle";
                    var errorMessage = "Algorithm failed. Error code: " + errorCode + ".";
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string generateOptimalListString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < visitOrder.Count; i++)
            {
                sb.Append(btsList.ElementAt(visitOrder[i]).cityGoogleString);
                sb.Append(Environment.NewLine);
            }

            sb.Append("Distance: " + visitOrderDistance.ToString() + " km.");

            return sb.ToString();
        }

        private void buttonDeleteBtsFromList_Click(object sender, RoutedEventArgs e)
        {
            if (onlyOneRowSelected(dataGridList))
            {
                Database.Bt btsToRemove = (Database.Bt)dataGridList.SelectedItem;
                btsList.Remove(btsToRemove);
                updateDataGridList();
            }
            else
            {
                MessageBox.Show("Only one row can be selected!");
            }
        }

        private void buttonAddBtsToList_Click(object sender, RoutedEventArgs e)
        {
            if (onlyOneRowSelected(dataGridBts))
            {
                Database.Bt bts = (Database.Bt)dataGridBts.SelectedItem;
                btsList.Add(bts);
                updateDataGridList();
            }
            else
            {
                MessageBox.Show("Only one row can be selected!");
            }
        }

        private void updateDataGridList()
        {
            dataGridList.ItemsSource = "{Binding}";
            dataGridList.ItemsSource = btsList;
        }

        private bool onlyOneRowSelected(DataGrid dataGrid)
        {
            return dataGrid.SelectedItems.Count == 1 ? true : false;
        }

        private void drawOptimalRouteOnMap()
        {
            if (btsList.Count != (visitOrder.Count - 1))
            {
                throw new Exception("Optimal path does not contain all locations");
            }

            clearMap();

            Database.Bt start;
            Database.Bt end;

            for (int i = 0; i < btsList.Count; i++)
            {
                start = btsList.ElementAt(visitOrder[i]);
                end = btsList.ElementAt(visitOrder[i+1]);
                drawSingleRoute(start, end);
            }
        }

        private void clearMap()
        {
            var clear = gmap.Markers.Where(p => p != null);
            if (clear != null)
            {
                for (int i = 0; i < clear.Count(); i++)
                {
                    gmap.Markers.Remove(clear.ElementAt(i));
                    i--;
                }
            }
        }

        private void drawSingleRoute(Database.Bt start, Database.Bt end)
        {
            PointLatLng startCords = new PointLatLng((double)start.latitude, (double)start.longtitude);
            PointLatLng endCords = new PointLatLng((double)end.latitude, (double)end.longtitude);

            RoutingProvider rp = gmap.MapProvider as RoutingProvider;
            MapRoute route = rp.GetRoute(startCords, endCords, false, false, (int)gmap.Zoom);

            GMapRoute mRoute = new GMapRoute(route.Points);
            {
                mRoute.ZIndex = -1;
            }
            gmap.Markers.Add(mRoute);
            gmap.ZoomAndCenterMarkers(null);
        }
    }
}
