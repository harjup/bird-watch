using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.src.Code.Models
{
    public static class BirdListing
    {
        //TODO: Should battle stats be modeled here?
        public static Bird SJ = new Bird("SJ");
        public static Bird WS = new Bird("WS");
        public static Bird AW = new Bird("AW");
        public static Bird AK = new Bird("AK");
        public static Bird BT = new Bird("BT");

        public static Bird CM = new Bird("CM");
        public static Bird EG = new Bird("EG");
        public static Bird JD = new Bird("JD");
        public static Bird NS = new Bird("NS");

        private static Bird _current;

        public static List<Bird> DayBirds = new List<Bird>
        {
            SJ,
            AW,
            AK,
            WS,
            BT
        };

        public static List<Bird> NightBirds = new List<Bird>
        {
            EG.At(Day.TimeOfDay.Night),
            JD.At(Day.TimeOfDay.Night),
            NS.At(Day.TimeOfDay.Night),
            SJ.At(Day.TimeOfDay.Night),
            AW.At(Day.TimeOfDay.Night)
        };

        public static List<Bird> RainBirds = new List<Bird>
        {
            CM.At(Day.TimeOfDay.Rain),
            WS.At(Day.TimeOfDay.Rain),
            AK.At(Day.TimeOfDay.Rain),
            JD.At(Day.TimeOfDay.Rain),
            NS.At(Day.TimeOfDay.Rain)
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
