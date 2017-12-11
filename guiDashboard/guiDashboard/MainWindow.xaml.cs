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
using System.Windows.Navigation;
using System.Windows.Shapes;
using sqLite;
using System.Data.SQLite;
using guiDashboard;

namespace guiDashboard
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         public MainWindow()
        {
            InitializeComponent();
            DBManager.testoutput();

            FetchData();
            
        }

        public static void FetchData()
        {
            //Consumption c = new Consumption();
            //c.trackid = 777;
            //c.time = "blubbb";
            //c.consumption = 9.5;


            using (SQLiteConnection db = new SQLiteConnection(@"Data Source=C:\Users\Kevin\Desktop\WS_1718\GUI\GUI Projekt Doks\Datenbank\consumption_monitoring.db;Version=3;New=True;Compress=True;"))
            {
                db.Open();
                var command = db.CreateCommand();

                command.CommandText = @"Select * from Track";

                SQLiteDataReader sdr = command.ExecuteReader();

                while (sdr.Read())
                {
                    Console.WriteLine(sdr.GetString(1) + " " + sdr.GetString(2));
                }
                sdr.Close();

                db.Close();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello World");
        }
    }
}
