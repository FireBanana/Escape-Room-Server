using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Unity_Escape_Room_Server_WPF.Windows;

public class Team
{
    public string Name { get; set; }
    public string FormattedTime;
    public int Score;
    public string FinalChoice { get; set; }
    public string FinalTime { get; set; }
    public int HintsUsed;
    public bool IsPaused;

    public int Time;

    Timer timer = new Timer(1000);

    public Team(string name)
    {
        Name = name;
        Time = 2700;
        timer.Elapsed += TimerElapsed;
        timer.Start();
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        Time -= 1;
        FormattedTime = Utilities.SecondsToFormattedString(Time);

        if(WindowManager.IsWindowOpen(Name))
        {
            var window = WindowManager.GetWindow(Name);
            var teamWindow = (TeamWindow)window;
            teamWindow.UpdateTimerText(FormattedTime);
        }
    }

    public void UpdatePoints(int newPoints)
    {
        Score = newPoints;

        if (WindowManager.IsWindowOpen(Name))
        {
            var window = WindowManager.GetWindow(Name);
            var teamWindow = (TeamWindow)window;
            teamWindow.UpdateScoreText(newPoints.ToString());
        }
    }

    
}

