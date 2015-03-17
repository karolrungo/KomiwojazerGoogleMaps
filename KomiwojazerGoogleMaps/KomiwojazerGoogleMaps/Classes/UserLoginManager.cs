using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomiwojazerGoogleMaps.Classes
{
    public class UserLoginManager
    {
        private Database.LinqDatabaseConnector databaseConnector;
        private DataTypes.UserData userData;

        public UserLoginManager(string username, string password, bool isAdmin)
        {
            userData = new DataTypes.UserData(username, password, isAdmin);
        }

        internal void logUserToMainMenu()
        {
            validateUserData();
        }

        private void validateUserData()
        {
            if (userData.Username.Contains(" "))
            {
                throw new LoginHasWhitespaces("ERROR!!! Login has whitespaces!");
            }
        }
    }
}
