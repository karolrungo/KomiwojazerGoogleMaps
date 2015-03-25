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

        public IQueryable<Bt> selectAllBts()
        {
            return dataContext.Bts.Select(x => x);
        }

        internal bool userExists(string username, string password, bool isAdmin)
        {
            return (from c in dataContext.Peoples 
                    where c.username == username &&
                          c.password == password &&
                          c.isAdmin == isAdmin
                    select c).Any();
        }

        public void deleteBTS(string location)
        {
            var itemToRemove = (from c in dataContext.Bts
                                where c.cityGoogleString == location
                                select c).First();

           dataContext.Bts.DeleteOnSubmit(itemToRemove);
           dataContext.SubmitChanges();    
        }

        internal void addBts(string city, string location, float latitude, float longitude)
        {
            Database.Bt bts= new Bt();
            bts.city = city;
            bts.cityGoogleString = location;
            bts.latitude = latitude;
            bts.longtitude = longitude;

            if (isBtsAlreadyInDatabase(location))
            {
                dataContext.Bts.InsertOnSubmit(bts);
                dataContext.SubmitChanges();
            }
        }

        private bool isBtsAlreadyInDatabase(string location)
        {
            var query = from c in dataContext.Bts
                        where c.cityGoogleString == location
                        select c;

            if (query.Any())
            {
                throw new Exception("This location ius already in database");
            }
            else
            {
                return true;
            }
        }
    }
}
