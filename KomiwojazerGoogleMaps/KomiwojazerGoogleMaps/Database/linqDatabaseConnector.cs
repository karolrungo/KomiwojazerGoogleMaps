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

        public bool userExists(string username, string password, bool isAdmin)
        {
            return (from c in dataContext.Peoples 
                    where c.username == username &&
                          c.password == password &&
                          c.isAdmin == isAdmin
                    select c).Any();
        }

        public void addBts(string city, string location, float latitude, float longitude)
        {
            Database.Bt bts= new Bt();
            bts.city = city;
            bts.cityGoogleString = location;
            bts.latitude = latitude;
            bts.longtitude = longitude;

            if (btsNotInDatabase(location, latitude, longitude))
            {
                dataContext.Bts.InsertOnSubmit(bts);
                dataContext.SubmitChanges();
            }
        }

        public void deleteBTS(string location)
        {
            var itemToRemove = (from c in dataContext.Bts
                                where c.cityGoogleString == location
                                select c).First();

            dataContext.Bts.DeleteOnSubmit(itemToRemove);
            dataContext.SubmitChanges();
        }

        public void addUser(string login, string password, bool isAdmin)
        {
            Database.People user = new People();
            user.username = login;
            user.password = password;
            user.isAdmin = isAdmin;

            if (userNotInDatabase(login))
            {
                dataContext.Peoples.InsertOnSubmit(user);
                dataContext.SubmitChanges();
            }
        }

        public void deleteUser(string login)
        {
            var itemToRemove = (from c in dataContext.Peoples
                                where c.username == login
                                select c).First();

            dataContext.Peoples.DeleteOnSubmit(itemToRemove);
            dataContext.SubmitChanges();
        }

        private bool btsNotInDatabase(string location, float latitude, float longitude)
        {
            var query = from c in dataContext.Bts
                        where c.cityGoogleString == location &&
                              c.longtitude == longitude &&
                              c.latitude == latitude
                        select c;

            if (query.Any())
            {
                throw new Exception("This location is already in database");
            }
            else
            {
                return true;
            }
        }

        private bool userNotInDatabase(string login)
        {
            var query = from c in dataContext.Peoples
                        where c.username == login
                        select c;

            if (query.Any())
            {
                throw new Exception("Login already exists in database");
            }
            else
            {
                return true;
            }
        }
    }
}
