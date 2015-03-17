using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KomiwojazerGoogleMaps.Classes
{
    public class LoginHasWhitespaces : Exception
    {
        public LoginHasWhitespaces(string message) : base(message)
        {}
    }
}
