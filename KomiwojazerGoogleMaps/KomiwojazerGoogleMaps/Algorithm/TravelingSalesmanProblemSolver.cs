using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomiwojazerGoogleMaps.Algorithm
{
    public class TravelingSalesmanProblemSolver
    {
        List<Database.Bt> btsList;

        public TravelingSalesmanProblemSolver(HashSet<Database.Bt> btsList)
        {
            //zapraszam do tańca ;)
            this.btsList = btsList.ToList();
        }

        public List<int> getOptimalOrder()
        {
            //narazie optymalna kolejnoscia jest kolejnosc na liscie
            return Enumerable.Range(0, btsList.Count-1).ToList();
        }
    }
}
