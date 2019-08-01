using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Unity_Escape_Room_Server_WPF.Windows
{
    /// <summary>
    /// Interaction logic for RoomScoreboard.xaml
    /// </summary>
    public partial class RoomScoreboard : Window
    {
        Team currentTeam;

        public RoomScoreboard(Team team)
        {
            InitializeComponent();
            currentTeam = team;
        }

        public void Reset(Team team)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ScoreText.Content = "0";
                RemainingTimeText.Content = "45:00";
                TeamName.Content = team.Name;
                HintBotText.Text = "";
                currentTeam = team;
            });
        }

        public void Tick(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ScoreText.Content = currentTeam.Score;
            RemainingTimeText.Content = currentTeam.FormattedTime;
            });
        }

        /// <summary>
        /// For manually setting points
        /// </summary>
        public void SetPoints(int points)
        {
            Dispatcher.Invoke(() =>
            {
                ScoreText.Content = points;
            });
        }

        public void DisplayMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                HintBotText.Text = message;
            });
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ScoreboardWindow.WindowState != WindowState.Maximized)
                ScoreboardWindow.WindowState = WindowState.Maximized;
            else
                ScoreboardWindow.WindowState = WindowState.Normal;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
            else
            {
                WindowManager.SetWindowOpenState("scoreboard", false, this);
                this.Close();
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            WindowManager.SetWindowOpenState("scoreboard", false, this);
        }
    }
}
