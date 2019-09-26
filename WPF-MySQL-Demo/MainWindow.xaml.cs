using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_MySQL_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ScanStatusKeysInBackgroundAsync();
            FillTable();

        }

        public void CheckStatusKeys()
        {
            var isNumLockToggled = Keyboard.IsKeyToggled(Key.NumLock);
            var isCapsLockToggled = Keyboard.IsKeyToggled(Key.CapsLock);
            var isScrollLockToggled = Keyboard.IsKeyToggled(Key.Scroll);

            if (isNumLockToggled)
            {
                NumLockStatus.Foreground = Brushes.Red;
            }
            else
            {
                NumLockStatus.Foreground = Brushes.Gray;
            }

            if (isCapsLockToggled)
            {
                CapsLockStatus.Foreground = Brushes.Red;
            }

            else
            {
                ScrollLockStatus.Foreground = Brushes.Gray;

            }
        }


        private async Task ScanStatusKeysInBackgroundAsync()
        {
            while (true)
            {
                CheckStatusKeys();
                await Task.Delay(100);
            }
        }

        private void FilterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            FillTable(FilterTextBox.Text);

        }

        private async void FillTable(string searchTerm = "")
        {
            //Data Source String (Data Source Name)
            string connStr =
                "server=localhost;" +
                "user=nmt_demo_user;" +
                "database=nmt_demo;" +
                "port=3306;" +
                "password=Password1";

            try
            {
                MessageTextBlock.Text = "Configuring database connection";
                await Task.Delay(250);

                //Perform database operations
                string sql = "SELECT * FROM locations";
                if (!searchTerm.Equals(""))
                {
                    sql += "WHERE location_name LIKE '%" + searchTerm + "%'";

                }
                MessageTextBox.Text = sql;
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    MessageTextBox.Text = "Processing...";

                    await Task.Delay(1250);
                    using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
                    {
                        DataTable dt = new DataTable();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                        da.Fill(dt);
                        LocationDataGrid.DataContext = dt;
                    }
                    connection.Close();
                    MessageTextBlock.Text = "Ready";
                }
            }
            catch(Exception ex)
            {
                MessageTextBlock.Text = ex.ToString();
            }
        }
    }

    //private void FilterTextBox_TextChanged(object sender,TextChangedEventArgs e)




}
