using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatharsisRawWebApplication.Service
{
    public class RegenerationStringMethod
    {
        public string RegenerationString()
        {

            Random random = new Random();
            string resultIdString = "";
            string[] simbols = {"1","2","3","4","5","6","7",
                "8","9","0","a","b","c","d","e","f","g"};
            for (int i = 0; i < 8; i++)
            {
                resultIdString += simbols[random.Next(0, 17)];
            }
            return resultIdString;
          
        }
    }
}
