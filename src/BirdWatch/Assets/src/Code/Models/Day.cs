using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Day
{
    public Day(TimeOfDay time, List<string> dialogBetweenEncounters, List<Bird> availableBirds)
    {
        Time = time;
        DialogBetweenEncounters = dialogBetweenEncounters;
        AvailableBirds = availableBirds;
    }

    public enum TimeOfDay
    {
        Day,
        Night,
        Rain
    }

    public TimeOfDay Time { get; private set; }

    public List<string> DialogBetweenEncounters { get; private set; }

    public List<Bird> AvailableBirds { get; private set; } 

    // Extensions:
        // Get next dialog event
        // Get options for encounter
}

public static class DayExtensions
{
    public static GameObject GetFieldBackground(this Day day)
    {
        switch (day.Time)
        {
            case Day.TimeOfDay.Day:
                return Resources.Load<GameObject>("Prefabs/Field/field-background-day");
            case Day.TimeOfDay.Night:
                return Resources.Load<GameObject>("Prefabs/Field/field-background-night");
            case Day.TimeOfDay.Rain:
                return Resources.Load<GameObject>("Prefabs/Field/field-background-rain");
        }

        return Resources.Load<GameObject>("Prefabs/Field/field-background-day");
    }

    public static GameObject GetEncounterBackground(this Day day)
    {
        // TODO: IMPLEMENT
        return null;
    }



}
