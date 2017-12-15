using System;
using System.Data.SQLite;

namespace guiDashboard
{

    class DBManager
    {
        static string _path = @"Data Source=C:\Users\Kevin\Desktop\WS_1718\GUI\GUI Projekt Doks\Datenbank\consumption_monitoring.db;Version=3;New=True;Compress=True;";

        public DBManager(string path)
        {
            _path = path;
        }

        public static void Create()
        {
            using (SQLiteConnection db = new SQLiteConnection(_path))
            {
                //db.CreateTable<Consumption>();
                //db.CreateTable<Track>();
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

                Console.WriteLine("Hello");
            }
        }

        //Query: Durchschnittsverbrauch pro Tour
        //Select c.trackid as TrackID, (select sum(consumption) FROM Consumption WHERE trackid = c.trackid)/11 as SUM_Consumption_ByID
        //FROM Consumption AS c
    }
}
