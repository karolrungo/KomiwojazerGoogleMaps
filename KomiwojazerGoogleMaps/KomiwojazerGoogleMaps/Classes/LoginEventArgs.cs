using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KomiwojazerGoogleMaps.Classes
{
    public class LoginEventArgs : EventArgs
    {
        public bool isAdmin;

        public LoginEventArgs(bool isAdmin)
        {
            this.isAdmin = isAdmin;
        }
    }
}
