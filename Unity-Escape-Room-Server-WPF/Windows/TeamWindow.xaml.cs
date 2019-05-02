using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// Interaction logic for TeamWindow.xaml
    /// </summary>
    public partial class TeamWindow : Window
    {
        public string TeamName;
        Team currentTeam;

        public TeamWindow(Team team)
        {
            InitializeComponent();

            TeamName = team.Name;
            TeamNameText.Content = team.Name;
            TimeRemainingText.Content = team.FormattedTime;
            ScoreText.Content = team.Score;

            currentTeam = team;

            FinalChoiceText.Content = team.FinalChoice;
            FinalTimeText.Content = team.FinalTime;
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            WindowManager.SetWindowOpenState(TeamName, false, this);
        }

        public void UpdateTimerText(string text)
        {
            if(TimeRemainingText.Dispatcher.Thread == Thread.CurrentThread)
                TimeRemainingText.Content = text;
            else
            {
                TimeRemainingText.Dispatcher.Invoke(() => { TimeRemainingText.Content = text; });
            }
        }

        public void UpdateScoreText(string text)
        {
            if (ScoreText.Dispatcher.Thread == Thread.CurrentThread)
                ScoreText.Content = text;
            else
            {
                ScoreText.Dispatcher.Invoke(() => { ScoreText.Content = text; });
            }
        }

        private void OnPreMadeHintSend(object sender, RoutedEventArgs e)
        {
            if(HintListBox.SelectedItem != null)
            {
                var hint = (string)HintListBox.SelectedItem;
                NetworkHandler.Instance.SendHintResponse(TeamName, hint);
            }
        }

        private void OnCustomHintSend(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(HintTextBox.Text))
            {
                var hint = HintTextBox.Text;
                NetworkHandler.Instance.SendHintResponse(TeamName, hint);
            }
            else
            {
                MessageBox.Show("Please enter hint");
            }
        }

        private void OnPauseClick(object sender, RoutedEventArgs e)
        {
            if(currentTeam.IsPaused)
            {
                currentTeam.IsPaused = false;
            }
            else
            {
                currentTeam.IsPaused = true;
            }

            NetworkHandler.Instance.SendPauseCommand(TeamName, currentTeam.IsPaused);
        }
    }
}
