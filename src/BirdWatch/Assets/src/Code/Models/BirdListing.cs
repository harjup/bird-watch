using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.src.Code.Models
{
    public static class BirdListing
    {
        private static Bird _current;

        private static List<Bird> DayBirds = new List<Bird>
        {
                //TODO: Should battle stats be modeled here?
                new Bird("AW", "Acorn Woodpecker", 20),
                new Bird("AK", "American Kestrel", 20),
                new Bird("BT", "Bushtit", 20)
        };

        public static Bird GetNextDayBird()
        {
            if (_current == null)
            {
                _current = DayBirds.First();
                return _current;
            }

            var index = DayBirds.IndexOf(_current) + 1;

            if (index >= DayBirds.Count)
            {
                index = 0;
            }

            _current = DayBirds[index];

            return _current;
        }

        public static Bird GetCurrentDayBird()
        {
            if (_current == null)
            {
                _current = DayBirds.First();
            }

            return _current;
        }

//
//        public static List<Bird> GetDayBirds()
//        {
//            return new List<Bird>
//            {
//                //TODO: Should battle stats be modeled here?
//                new Bird("AW", "Acorn Woodpecker", 20),
//                new Bird("AK", "American Kestrel", 20),
//                new Bird("BT", "Bushtit", 20)
//            };
//        } 


    }
}
