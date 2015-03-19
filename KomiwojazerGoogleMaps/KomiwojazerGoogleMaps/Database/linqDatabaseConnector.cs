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

        public IQueryable<People> selectAllUsers()
        {
            return dataContext.Peoples.Select(user => user);
        }

        internal bool userExists(string username, string password, bool isAdmin)
        {
            return (from c in dataContext.Peoples 
                    where c.username == username &&
                          c.password == password &&
                          c.isAdmin == isAdmin
                    select c).Any();
        }
    }
}
