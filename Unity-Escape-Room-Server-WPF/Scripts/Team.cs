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
    public string FormattedElapsedTime { get; set; }
    public int Score { get; set; }
    public string FinalChoice { get; set; }
    public string FinalTime { get; set; }
    public int HintsUsed;
    public bool IsPaused;

    public int Time;
    public int ElapsedTime;

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
        ElapsedTime += 1;
        FormattedTime = Utilities.SecondsToFormattedString(Time);
        FormattedElapsedTime = Utilities.SecondsToFormattedString(ElapsedTime);

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

    public void Stop()
    {
        timer.Stop();
        FinalTime = FormattedElapsedTime;

        if (WindowManager.IsWindowOpen(Name))
        {
            var window = WindowManager.GetWindow(Name);
            var teamWindow = (TeamWindow)window;

            teamWindow.UpdateFinalItems(FinalChoice, Score.ToString(), FinalTime);
        }
    }

    
}

