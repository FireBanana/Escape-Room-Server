using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Utilities
{
    public static string SecondsToFormattedString(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        if(time.Seconds > 10)
            return time.Minutes + ":" + time.Seconds;
        else
            return time.Minutes + ":0" + time.Seconds;
    }

    public static int GetMinutes(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.Minutes;
    }
}
