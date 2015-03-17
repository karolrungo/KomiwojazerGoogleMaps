using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomiwojazerGoogleMaps.Database
{
    public class LinqDatabaseConnector
    {
        private DatabaseContextDataContext dataContext;

        public LinqDatabaseConnector()
        {
            dataContext = new DatabaseContextDataContext();
        }

        public IQueryable<User> selectAllUsers()
        {
            return dataContext.Users.Select(user => user);
        }
    }
}
