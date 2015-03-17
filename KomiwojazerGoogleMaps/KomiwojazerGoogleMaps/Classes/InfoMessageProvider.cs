using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KomiwojazerGoogleMaps.Classes
{
    public static class InfoMessageProvider
    {
        public static void showInfoMessage()
        {
            StringBuilder infoMessage = new StringBuilder();
            infoMessage.Append("Optymalizacja dyskretnych procesów produkcyjnych - Projekt lato 2014/2015").Append(Environment.NewLine);
            infoMessage.Append("Autorzy:").Append(Environment.NewLine);
            infoMessage.Append("Sebastian Kowalewski, Bartłomiej Kozdraś, Karol Rungo").Append(Environment.NewLine);
            infoMessage.Append("Prowadzący: dr inż. Jarosław Pempera");
            MessageBox.Show(infoMessage.ToString());
        }
    }
}
