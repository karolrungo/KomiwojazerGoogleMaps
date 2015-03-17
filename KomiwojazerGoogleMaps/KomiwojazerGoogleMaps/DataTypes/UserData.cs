using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomiwojazerGoogleMaps.DataTypes
{
    public class UserData
    {
        private string _username;
        private string _password;
        private bool _isAdmin;

        public string Username { get { return _username; } set { _username = value; } }
        public string Password { get { return _password; } set { _password = value; } }
        public bool IsAdmin { get { return _isAdmin; } set { _isAdmin = value; } }

        public UserData(string username, string password, bool isAdmin)
        {
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
        }
    }
}
