using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace KomiwojazerGoogleMaps.Classes
{
    public static class BtsFinder
    {
        public static DataTable findLocations(string city)
        {
            WebRequest request = createRequest(city);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);

                    DataTable dtCoordinates = new DataTable();
                    setDataTableColumns(dtCoordinates);
                    fillDatatable(dsResult, dtCoordinates);
                    return dtCoordinates;
                }
            }
        }

        public static double getDistanceBetweenLocations(Database.Bt start, Database.Bt end)
        {
            if (start == end)
            {
                throw new Exception("Error in: getDistanceBetweenLocations. start == end");
            }
            HttpWebRequest request = createRequestForCalculatingDistance(start, end);
            string responsereader = getResponse(request);
            return getDistanceFromXml(responsereader);
        }

        private static double getDistanceFromXml(string responsereader)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);

            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                return Convert.ToDouble(distance[0].ChildNodes[1].InnerText.Replace(" km", ""));
            }

            throw new Exception("Error in: getDistanceFromXml");
        }

        private static string getResponse(HttpWebRequest request)
        {
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            return responsereader;
        }

        private static HttpWebRequest createRequestForCalculatingDistance(Database.Bt start, Database.Bt end)
        {
            string url = @"http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" +
                          start.cityGoogleString + "&destinations=" + end.cityGoogleString +
                          "&mode=driving&sensor=false&language=en-EN";
            return (HttpWebRequest)WebRequest.Create(url);
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

        private static void setDataTableColumns(DataTable dtCoordinates)
        {
            dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                        new DataColumn("Address", typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longitude",typeof(string)) });
        }
    }
}
