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

        public LobbyScreen()
        {
            InitializeComponent();
            LoadItemsToTable();
        }

        public void LoadItemsToTable()
        {
            var items = Database.GetResults();

            var sortedList = items.OrderByDescending(x => int.Parse(x.Score));

            if (TeamDataGrid.Dispatcher.Thread != Thread.CurrentThread)
            {
                TeamDataGrid.Dispatcher.Invoke(() => { TeamDataGrid.Items.Clear(); });
            }
            else
            {
                TeamDataGrid.Items.Clear();
            }

                foreach (var client in sortedList)
            {
                if (TeamDataGrid.Dispatcher.Thread != Thread.CurrentThread)
                {
                    TeamDataGrid.Dispatcher.Invoke(() => { TeamDataGrid.Items.Add(client); });
                }
                else
                {
                    TeamDataGrid.Items.Add(client);
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
                this.Close();
            }
        }
    }
}
