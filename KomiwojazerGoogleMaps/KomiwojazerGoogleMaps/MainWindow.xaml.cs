using GMap.NET;
using GMap.NET.WindowsPresentation;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize map:
            gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            //gmap.Position = new PointLatLng(-25.971684, 32.589759);
            gmap.SetPositionByKeywords("Wołów, Poland");

            PointLatLng start = new PointLatLng(51.110, 17.030);
            PointLatLng end = new PointLatLng(52.259, 21.020);

            RoutingProvider rp = gmap.MapProvider as RoutingProvider;
            MapRoute route = rp.GetRoute(start, end, false, false, (int)gmap.Zoom);

            GMapMarker m1 = new GMapMarker(start);
            GMapMarker m2 = new GMapMarker(end);
            GMapRoute mRoute = new GMapRoute(route.Points);
            gmap.Markers.Add(mRoute);
            gmap.ZoomAndCenterMarkers(null);
        }
    }
}
