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
using System.Data.SQLite;
using guiDashboard;
using System.Net.Sockets;
using System.Net;
using sqLite;

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
            DBManager.Create();

            StartListener();
        }


        //public static void FetchData()
        //{
        //    //Consumption c = new Consumption();
        //    //c.trackid = 777;
        //    //c.time = "blubbb";
        //    //c.consumption = 9.5;

        //    using (SQLiteConnection db = new SQLiteConnection(@"Data Source=C:\Users\Kevin\Desktop\WS_1718\GUI\GUI Projekt Doks\Datenbank\consumption_monitoring.db;Version=3;New=True;Compress=True;"))
        //    {
        //        db.Open();
        //        var command = db.CreateCommand();

        //        command.CommandText = @"Select * from Track";

        //        SQLiteDataReader sdr = command.ExecuteReader();

        //        while (sdr.Read())
        //        {
        //            Console.WriteLine(sdr.GetString(1) + " " + sdr.GetString(2));
        //        }
        //        sdr.Close();

        //        db.Close();
        //    }
        //}

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello World");
        }

        private static void StartListener()
        {
            // This constructor arbitrarily assigns the local port number.
            UdpClient udpClient = new UdpClient();

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var deserializedResult = serializer.Deserialize<List<myJsonObj>>(serializedResult);



            try
            {
                udpClient.Connect("127.0.0.1", 8056);

                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes("START");

                udpClient.Send(sendBytes, sendBytes.Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);


                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received " +
                                             returnData.ToString());
                Console.WriteLine("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());

                udpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
