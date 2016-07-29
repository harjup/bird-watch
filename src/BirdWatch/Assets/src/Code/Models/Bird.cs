using System;
using UnityEngine;
using System.Collections;

public class Bird
{
    public enum EncounterBg
    {
        Mountain,
        Redwood,
        River
    }

    public string Id { get; private set; }

    public EncounterBg Background { get; private set; }

    public Day.TimeOfDay Time { get; private set; }
    
    public Bird(string id, EncounterBg bg, Day.TimeOfDay time = Day.TimeOfDay.Day)
    {
        Id = id;
        Background = bg;
        Time = time;
        
    }

    public Bird At(Day.TimeOfDay time)
    {
        return new Bird(Id, Background, time);
    }
    
    public string GetTimeDescription()
    {
        switch (Time)
        {
            case Day.TimeOfDay.Day:
                return "Day";
            case Day.TimeOfDay.Night:
                return "Night";
            case Day.TimeOfDay.Rain:
                return "Rain";
        }

        return "Day";
    }


    // Text store?

    // Photo

    // Sprite
}