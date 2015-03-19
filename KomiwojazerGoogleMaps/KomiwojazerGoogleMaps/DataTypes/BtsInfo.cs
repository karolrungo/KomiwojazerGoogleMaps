using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomiwojazerGoogleMaps.DataTypes
{
    public class BtsInfo
    {
        private string city;
        private string location;
        private float latitude;
        private float longitude;

        public string City { get { return city; } set { city = value; } }
        public string Location { get { return location; } set { location = value; } }
        public float Latitude { get { return latitude; } set { latitude = value; } }
        public float Longitude { get { return longitude; } set { longitude = value; } }

        public BtsInfo(string city, string location, float latitude, float longtitude)
        {
            City = city;
            Location = location;
            Latitude = latitude;
            Longitude = longtitude;
        }
    }
}
