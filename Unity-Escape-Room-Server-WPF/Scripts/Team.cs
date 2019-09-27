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
    public int Score { get { return totalScore; } }
    public string FinalChoice { get; set; }
    public string FinalTime { get; set; }
    public int HintsUsed;
    public bool IsPaused;

    public int Time;
    public int ElapsedTime;

    int hiddenScore { get; set; }
    int totalScore { get; set; }

    int previousMinute;

    public Timer Timer = new Timer(1000);
    // delegate void TimerEvent(int elapseTime);
    //public TimerEvent TimedEvent;

    public const int TotalTime = 180;

    public Team(string name)
    {
        Name = name;
        Time = TotalTime;
        Timer.Elapsed += TimerElapsed;
        Timer.Start();
        totalScore = 1500;

        //Start scoreboard
        if (WindowManager.IsWindowOpen("scoreboard"))
        {
            var scoreboardWindow = (RoomScoreboard)WindowManager.GetWindow("scoreboard");
            scoreboardWindow.Reset(this);
            //Timer.Elapsed += scoreboardWindow.Tick;
            Timer.Elapsed += scoreboardWindow.Tick;
        }
        else
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var scoreboardWindow = new RoomScoreboard(this);
                scoreboardWindow.Show();
                WindowManager.SetWindowOpenState("scoreboard", true, scoreboardWindow);
                scoreboardWindow.Reset(this);
                //Timer.Elapsed += scoreboardWindow.Tick;
                Timer.Elapsed += scoreboardWindow.Tick;
            });
        }

        RoomScoreboard.IsSet = true;
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        Time -= 1;
        ElapsedTime += 1;
        FormattedTime = Utilities.SecondsToFormattedString(Time);
        FormattedElapsedTime = Utilities.SecondsToFormattedString(ElapsedTime);

        var currentMinute = Utilities.GetMinutes(ElapsedTime);

        if (previousMinute != currentMinute)
        {
            previousMinute = currentMinute;
            totalScore -= 25;
        }

        if (WindowManager.IsWindowOpen(Name))
        {
            var window = WindowManager.GetWindow(Name);
            var teamWindow = (TeamWindow)window;
            teamWindow.UpdateTimerText(FormattedTime);
            teamWindow.UpdateScoreText(Score.ToString());
        }

        if (WindowManager.IsWindowOpen("scoreboard") && !RoomScoreboard.IsSet)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var scoreboardWindow = (RoomScoreboard)WindowManager.GetWindow("scoreboard");
                WindowManager.SetWindowOpenState("scoreboard", true, scoreboardWindow);
                scoreboardWindow.Reset(this);
                //Timer.Elapsed += scoreboardWindow.Tick;
                Timer.Elapsed += scoreboardWindow.Tick;
                RoomScoreboard.IsSet = true;
            });
        }

        if (Time <= 0)
        {
            totalScore -= hiddenScore;
            Timer.Stop();
            NetworkHandler.Instance.SendGameEnd(Name, Score);
        }
    }

    //public void TimerElapsed_(int elapsedTime)
    //{
    //    Time -= 1;
    //    ElapsedTime += 1;
    //    FormattedTime = Utilities.SecondsToFormattedString(TotalTime - elapsedTime);
    //    FormattedElapsedTime = Utilities.SecondsToFormattedString(elapsedTime);

    //    //var currentMinute = Utilities.GetMinutes(ElapsedTime);

    //    //if (previousMinute != currentMinute)
    //    //{
    //    //    previousMinute = currentMinute;
    //    //    Score -= 25;
    //    //}

    //    if (WindowManager.IsWindowOpen(Name))
    //    {
    //        var window = WindowManager.GetWindow(Name);
    //        var teamWindow = (TeamWindow)window;
    //        teamWindow.UpdateTimerText(FormattedTime);
    //        teamWindow.UpdateScoreText(Score.ToString());
    //    }
    //}

    public void UpdatePoints(int newPoints, bool isHidden)
    {
        if (isHidden)
            hiddenScore += Math.Abs(newPoints);
        else
            totalScore += newPoints;

        //if (WindowManager.IsWindowOpen(Name))
        //{
        //    var window = WindowManager.GetWindow(Name);
        //    var teamWindow = (TeamWindow)window;
        //    teamWindow.UpdateScoreText(newPoints.ToString());
        //}
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
            //Timer.Start();
        }
        else
        {
            IsPaused = true;
            //Timer.Stop();
        }
    }

    public void Stop()
    {        
        var finalPoints = totalScore;        

        try
        {
            if (WindowManager.IsWindowOpen(Name))
            {
                var window = WindowManager.GetWindow(Name);
                var teamWindow = (TeamWindow)window;

                teamWindow.UpdateFinalItems(FinalChoice, finalPoints.ToString(), FinalTime);                
            }

            if (WindowManager.IsWindowOpen("scoreboard"))
            {
                var roomScoreboard = (RoomScoreboard)WindowManager.GetWindow("scoreboard");
                roomScoreboard.SetPoints(finalPoints);
            }

            if (WindowManager.IsWindowOpen("lobby"))
            {
                var window = WindowManager.GetWindow("lobby");
                var lobbyWindow = (LobbyScreen)window;
                lobbyWindow.LoadItemsToTable();
            }

            Database.AddEntry(Name, finalPoints.ToString(), FinalTime);
        }
        catch (Exception e)
        {
            Task.Run(() => { MessageBox.Show("Error in Team: " + e.Message + "\n\n" + e.Data); });
        }
    }


}

