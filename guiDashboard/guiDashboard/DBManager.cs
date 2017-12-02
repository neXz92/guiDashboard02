using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace sqLite
{
    class DBManager
    {

        //Query: Durchschnittsverbrauch pro Tour
        //Select c.trackid as TrackID, (select sum(consumption) FROM Consumption WHERE trackid = c.trackid)/11 as SUM_Consumption_ByID
        //FROM Consumption AS c
        

        //outdated

        public static void testoutput()
        {
            //SQLiteConnection conn = new SQLiteConnection(@"Data Source=Data Source=C:\Users\Kevin\Desktop\consumption_monitoring.db;Version=3;New=True;Compress=True;");
            //conn.Open();

            //var command = conn.CreateCommand();

            //command.CommandText = @"Select * from Track";

            //SQLiteDataReader sdr = command.ExecuteReader();

            //while (sdr.Read())
            //{
            //    Console.WriteLine(sdr.GetString(1) + " " + sdr.GetString(2));
            //}
            //sdr.Close();

            //conn.Close();

            //Console.WriteLine("Hello");
        }
    }
}
