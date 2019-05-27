using System;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Unity_Escape_Room_Server_WPF
{
    /// <summary>
    /// Interaction logic for LobbyScreen.xaml
    /// </summary>
    public partial class LobbyScreen : Window
    {
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        delegate void OnAsyncTick();

        public LobbyScreen()
        {
            InitializeComponent();
            timer.Elapsed += Tick;
            timer.Start();
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            LoadItemsToTable();
        }

        public void LoadItemsToTable()
        {
            var sortedList = NetworkHandler.Instance.LobbyTeamsList.OrderBy(x => x.Value.Score);//.Select(p => p.Value).ToList();

            if (TeamDataGrid.Dispatcher.Thread != Thread.CurrentThread)
            {
                var del = new OnAsyncTick(() =>
                {
                    TeamDataGrid.Items.Clear();
                });
                TeamDataGrid.Dispatcher.BeginInvoke(del, null);
            }
            else
            {
                TeamDataGrid.Items.Clear();
            }

                foreach (var client in sortedList)
            {
                if (TeamDataGrid.Dispatcher.Thread != Thread.CurrentThread)
                {
                    var del = new OnAsyncTick(() =>
                    {                        
                        TeamDataGrid.Items.Add(client.Value);
                    });
                    TeamDataGrid.Dispatcher.BeginInvoke(del, null);
                }
                else
                {
                    TeamDataGrid.Items.Add(client.Value);
                }
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            WindowManager.SetWindowOpenState("lobby", false, this);
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LobbyWindow.WindowState != WindowState.Maximized)
                LobbyWindow.WindowState = WindowState.Maximized;
            else
                LobbyWindow.WindowState = WindowState.Normal;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
            else
            {
                WindowManager.SetWindowOpenState("lobby", false, this);
                timer.Stop();
                timer.Dispose();
                this.Close();
            }
        }
    }
}
