using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Net.Sockets;
using System.Net;

namespace sqLite
{

    public class myJsonObj
    {
        public int blink { get; set; }
        public double distance { get; set; }
        public double fuel { get; set; }
        public bool fullbeam { get; set; }
        public int gear { get; set; }
        public bool light { get; set; }
        public int outsideTemperature { get; set; }
        public double rpm { get; set; }
        public double speed { get; set; }
        public bool warnsignal { get; set; }
        public int wiperLevel { get; set; }
    }

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
