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
using Finisar.SQLite;

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

           // DBManager.testoutput();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection sqlite_conn;
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            // create a new database connection:
            sqlite_conn = new SQLiteConnection(@"Data Source=C:\Users\Kevin\Desktop\consumption_monitoring.db;Version=3;New=True;Compress=True;");

            // open the connection:
            sqlite_conn.Open();

            // create a new SQL command:
            sqlite_cmd = sqlite_conn.CreateCommand();

            // Let the SQLiteCommand object know our SQL-Query:
            sqlite_cmd.CommandText = "CREATE TABLE Track (id integer primary key autoincrement, startpoint text not null, destination text NOT NULL, durationinmin int NOT NULL, trackdate text NOT NULL, starttime text NOT NULL, endtime text NOT NULL);";
            sqlite_cmd.CommandText = @"Select * from Track";

            // Now lets execute the SQL ;D
            sqlite_cmd.ExecuteNonQuery();

            // Lets insert something into our new table:
            //sqlite_cmd.CommandText = "INSERT INTO test (id, text) VALUES (1, 'Test Text 1');";

            // And execute this again ;D
            //sqlite_cmd.ExecuteNonQuery();

            // Now the SQLiteCommand object can give us a DataReader-Object:
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            // The SQLiteDataReader allows us to run through the result lines:
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // Print out the content of the text field:
                MessageBox.Show(sqlite_datareader["id"].ToString());
            }

            // We are ready, now lets cleanup and close our connection:
            sqlite_conn.Close();
        }
    }
}
