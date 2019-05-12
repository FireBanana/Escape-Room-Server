using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            var sortedList = NetworkHandler.Instance.TeamsList.OrderBy(x => x.Value.Score).Select(p => p.Value).ToList();

            foreach (var client in sortedList)
            {
                TeamDataGrid.Items.Add(client);
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            WindowManager.SetWindowOpenState("lobby", false, this);
        }
    }
}
