using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace sqLiteTest
{
    class DBManager
    {
        //der folgende Code hat in einer Konsolenanwendung wunderbar geklappt, hier in einer .cs datei - im WPF Projekt, erkennt er die using-direktive (oben) schon nicht.
        static void Main(string[] args)
        {
            SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\Kevin\Desktop\WS 17-18\GUI\GUI Projekt Doks\Datenbank\consumption_monitoring.db");
            conn.Open();

            var command = conn.CreateCommand();

            //Daten sind schon in File nach "import"
            //command.CommandText = @"CREATE TABLE Track (
            //                        id integer primary key autoincrement,
            //                        startpoint text not null,
            //                        destination text NOT NULL,
            //                        durationinmin int NOT NULL,
            //                        trackdate text NOT NULL,
            //                        starttime text NOT NULL,
            //                        endtime text NOT NULL
            //                    );";
            //command.ExecuteNonQuery();

            //command.CommandText = @"INSERT INTO Track(id,startpoint,destination,durationinmin,trackdate,starttime,endtime) VALUES (1,'Zweibrücken, Amerikastraße 1','Kaiserslautern, Kammgarn',55,'13.11.2017','09:06','10:01');";
            //command.ExecuteNonQuery();

            command.CommandText = @"Select * from Track";
            SQLiteDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                Console.WriteLine(sdr.GetString(1) + " " + sdr.GetString(2));
            }
            sdr.Close();

            conn.Close();

            Console.WriteLine("Hello");
            Console.ReadKey();
        }
    }
}
