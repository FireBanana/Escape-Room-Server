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
        return time.Minutes + ":" + time.Seconds;
    }
}
