using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomiwojazerGoogleMaps.Classes
{
    public class UserLoginManager
    {
        public delegate void LoginEventHandler(object sender, LoginEventArgs e);
        public event LoginEventHandler loginSusses;
        public event LoginEventHandler loginFailed;

        private Database.LinqDatabaseConnector databaseConnector;
        private DataTypes.UserData userData;

        public UserLoginManager(string username, string password, bool isAdmin)
        {
            userData = new DataTypes.UserData(username, password, isAdmin);
            databaseConnector = new Database.LinqDatabaseConnector();
        }

        internal void logUserToMainMenu()
        {
            validateUserData();

            if (databaseConnector.userExists(userData.Username, userData.Password, userData.IsAdmin))
            {
                raiseLoginSuccessEvent();
            }
            else
            {
                raiseLoginFailedEvent();
            }
        }

        private void raiseLoginFailedEvent()
        {
            if (loginFailed != null)
            {
                loginFailed(this, new LoginEventArgs(userData.IsAdmin));
            }
        }

        private void raiseLoginSuccessEvent()
        {
            if (loginSusses != null)
            {
                loginSusses(this, new LoginEventArgs(userData.IsAdmin));
            }
        }

        public void validateUserData()
        {
            if (userData.Username.Contains(" ") || userData.Username.Contains("\t"))
            {
                throw new LoginHasWhitespaces("ERROR!!! Login has whitespaces!");
            }

            if (userData.Password.Contains(" ") || userData.Password.Contains("\t"))
            {
                throw new LoginHasWhitespaces("ERROR!!! Password has whitespaces!");
            }
        }
    }
}
