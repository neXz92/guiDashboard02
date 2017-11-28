using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SQLite;
using Finisar.SQLite;


namespace sqLite
{
    class DBManager
    {
        //der folgende Code hat in einer Konsolenanwendung wunderbar geklappt, hier in einer .cs datei - im WPF Projekt, erkennt er die using-direktive (oben) schon nicht.
        //static void Main(string[] args)
        //{

        //}

        public static SQLiteConnection sqlite_conn;
        public static SQLiteCommand sqlite_cmd;
        public static SQLiteDataReader sqlite_datareader;

        public static void testoutput()
        {
            sqlite_conn = new SQLiteConnection(@"Data Source=C:\Users\Kevin\Desktop\consumption_monitoring.db;Version=3;New=True;Compress=True;");

            sqlite_conn.Open();

            var sqlite_cmd = sqlite_conn.CreateCommand();

            //Daten sind schon in File nach "import"
            sqlite_cmd.CommandText = "CREATE TABLE Track (id integer primary key autoincrement, startpoint text not null, destination text NOT NULL, durationinmin int NOT NULL, trackdate text NOT NULL, starttime text NOT NULL, endtime text NOT NULL);";

            sqlite_cmd.ExecuteNonQuery();

            //Git test von lappi
            sqlite_cmd.CommandText = @"Select * from Track";

            sqlite_cmd.ExecuteNonQuery();

            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                Console.WriteLine(sqlite_datareader.GetString(1) + " " + sqlite_datareader.GetString(2));
            }
            sqlite_datareader.Close();

            sqlite_conn.Close();


            Console.WriteLine("Hello");
            //Console.ReadKey();
        }
    }
}
