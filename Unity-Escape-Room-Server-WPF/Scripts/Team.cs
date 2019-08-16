using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Unity_Escape_Room_Server_WPF;
using Unity_Escape_Room_Server_WPF.Windows;

public class Team
{
    public string Name { get; set; }
    public string FormattedTime;
    public string FormattedElapsedTime { get; set; }
    public int Score { get { return totalScore + hiddenScore; } }
    public string FinalChoice { get; set; }
    public string FinalTime { get; set; }
    public int HintsUsed;
    public bool IsPaused;

    public int Time;
    public int ElapsedTime;

    int hiddenScore { get; set; }
    int totalScore { get; set; }

    //Timer timer = new Timer(1000);
    public delegate void TimerEvent(object sender, ElapsedEventArgs e);
    public TimerEvent TimedEvent;
      
    public Team(string name)
    {
        Name = name;
        Time = 180;
        //timer.Elapsed += TimerElapsed;
        //timer.Start();
        TimedEvent += TimerElapsed;
        totalScore = 1500;

        //Start scoreboard
        if (WindowManager.IsWindowOpen("scoreboard"))
        {
            var scoreboardWindow = (RoomScoreboard)WindowManager.GetWindow("scoreboard");
            scoreboardWindow.Reset(this);
            //timer.Elapsed += scoreboardWindow.Tick;
            TimedEvent += scoreboardWindow.Tick;
        }
        else
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {                
                var scoreboardWindow = new RoomScoreboard(this);
                scoreboardWindow.Show();
                WindowManager.SetWindowOpenState("scoreboard", true, scoreboardWindow);
                scoreboardWindow.Reset(this);
                //timer.Elapsed += scoreboardWindow.Tick;
                TimedEvent += scoreboardWindow.Tick;
            });
        }
    }

    public void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        Time -= 1;
        ElapsedTime += 1;
        FormattedTime = Utilities.SecondsToFormattedString(Time);
        FormattedElapsedTime = Utilities.SecondsToFormattedString(ElapsedTime);

        //var currentMinute = Utilities.GetMinutes(ElapsedTime);

        //if (previousMinute != currentMinute)
        //{
        //    previousMinute = currentMinute;
        //    Score -= 25;
        //}

        if(WindowManager.IsWindowOpen(Name))
        {
            var window = WindowManager.GetWindow(Name);
            var teamWindow = (TeamWindow)window;
            teamWindow.UpdateTimerText(FormattedTime);
            teamWindow.UpdateScoreText(Score.ToString());
        }
    }

    public void UpdatePoints(int newPoints, bool isHidden) 
    {
        if (isHidden)
            hiddenScore += Math.Abs(newPoints - totalScore);

        totalScore = newPoints;

        if (WindowManager.IsWindowOpen(Name))
        {
            var window = WindowManager.GetWindow(Name);
            var teamWindow = (TeamWindow)window;
            teamWindow.UpdateScoreText(newPoints.ToString());
        }
    }

    public void Pause()
    {
        if (WindowManager.IsWindowOpen(Name))
        {
            var window = WindowManager.GetWindow(Name);
            var teamWindow = (TeamWindow)window;
            teamWindow.ChangePauseStatus(!IsPaused);
        }

        //toggle
        if (IsPaused)
        {
            IsPaused = false;
            //timer.Start();
        }
        else
        {
            IsPaused = true;
            //timer.Stop();
        }
    }

    public void Stop(string time)
    {
        try
        {
            if (time != null)
            {
                var deviceTime = int.Parse(time);
                ElapsedTime = deviceTime - 1;

                //Time = 3601 - deviceTime;
            }

            //timer.Stop();
            TimerElapsed(null, null);
            FinalTime = FormattedElapsedTime;

            if (WindowManager.IsWindowOpen(Name))
            {
                var window = WindowManager.GetWindow(Name);
                var teamWindow = (TeamWindow)window;

                teamWindow.UpdateFinalItems(FinalChoice, (totalScore).ToString(), FinalTime);
                Database.AddEntry(Name, (totalScore).ToString(), FinalTime);
            }

            if (WindowManager.IsWindowOpen("scoreboard"))
            {
                var roomScoreboard = (RoomScoreboard)WindowManager.GetWindow("scoreboard");
                roomScoreboard.SetPoints(totalScore);
            }

            if (WindowManager.IsWindowOpen("lobby"))
            {
                var window = WindowManager.GetWindow("lobby");
                var lobbyWindow = (LobbyScreen)window;
                lobbyWindow.LoadItemsToTable();
            }
        }
        catch(Exception e)
        {
            Task.Run(() => { MessageBox.Show("Error in Team: " + e.Message + "\n\n" + e.Data); });
        }
    }

    
}

