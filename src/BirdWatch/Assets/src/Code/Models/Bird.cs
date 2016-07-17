using UnityEngine;
using System.Collections;

public class Bird
{
    public string Id { get; private set; }
    
    public Day.TimeOfDay Time { get; private set; }

    public Bird(string id, Day.TimeOfDay time = Day.TimeOfDay.Day)
    {
        Id = id;
        Time = time;
    }

    public Bird At(Day.TimeOfDay time)
    {
        return new Bird(Id, time);
    }


    // Text store?

    // Photo

    // Sprite
}