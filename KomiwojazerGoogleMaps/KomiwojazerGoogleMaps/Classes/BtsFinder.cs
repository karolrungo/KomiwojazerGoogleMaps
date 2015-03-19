using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KomiwojazerGoogleMaps.Classes
{
    public static class BtsFinder
    {
        public static DataTable findAddressess(string city)
        {
            WebRequest request = createRequest(city);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);

                    DataTable dtCoordinates = new DataTable();
                    setDatatableColumns(dtCoordinates);
                    fillDatatable(dsResult, dtCoordinates);
                    return dtCoordinates;
                }
            }
        }

        private static WebRequest createRequest(string city)
        {
            string url = "http://maps.google.com/maps/api/geocode/xml?address=" + city + "&sensor=false";
            WebRequest request = WebRequest.Create(url);
            return request;
        }

        private static void fillDatatable(DataSet dsResult, DataTable dtCoordinates)
        {
            try
            {
                foreach (DataRow row in dsResult.Tables["result"].Rows)
                {
                    string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                    DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                    dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No results found!");
            }
        }

        private static void setDatatableColumns(DataTable dtCoordinates)
        {
            dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                        new DataColumn("Address", typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longitude",typeof(string)) });
        }
    }
}
