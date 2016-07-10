using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.src.Code.Models
{
    public static class BirdListing
    {
        public static List<Bird> GetDayBirds()
        {
            return new List<Bird>
            {
                //TODO: Should battle stats be modeled here?
                new Bird("AW", "Acorn Woodpecker", 20),
                new Bird("AK", "American Kestrel", 20),
                new Bird("BT", "Bushtit", 20)
            };
        } 


    }
}
