using UnityEngine;
using System.Collections.Generic;
using Assets.src.Code.Models;

public class DayListing
{
    private static List<Day> _days = new List<Day>
        {
            new Day(Day.TimeOfDay.Day, DialogListing.DayDialog(), BirdListing.DayBirds),
            new Day(Day.TimeOfDay.Night, DialogListing.NightDialog(), BirdListing.NightBirds),
            new Day(Day.TimeOfDay.Rain, DialogListing.RainDialog(), BirdListing.RainBirds),
        };

    /// <summary>
    /// Sets the current day metadata, starting at 1
    /// </summary>
    public Day GetDay(int currentDay)
    {
        return _days[currentDay - 1];
    }
}
