using UnityEngine;
using System.Collections.Generic;

public static class DialogListing
{
    public static List<string> DayDialog()
    {
        return new List<string>
        {
            "Day_Chat_01",
            "Day_Chat_02",
            "Day_Chat_03",
            "Day_Chat_04",
            "Day_Chat_05",
        };
    }

    public static List<string> NightDialog()
    {
        return new List<string>
        {
            "Night_Chat_01",
            "Night_Chat_02",
            "Night_Chat_03",
            "Night_Chat_04",
            "Night_Chat_05",
        };
    }

    public static List<string> RainDialog()
    {
        return new List<string>
        {
            "Rain_Chat_01",
            "Rain_Chat_02",
            "Rain_Chat_03",
            "Rain_Chat_04",
            "Rain_Chat_05",
        };
    }
}
