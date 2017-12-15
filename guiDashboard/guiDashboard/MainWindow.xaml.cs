using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace guiDashboard
{
    public sealed partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Speed { get; private set; }
        public int SpeedometerNeedleRotation { get; private set; }

        private bool _receive = true;
        
        public MainWindow()
        {
            InitializeComponent();
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

        private void StartListener()
        {
            Task.Run(async () =>
            {
                using (var client = new UdpClient())
                {
                    client.Connect("localhost", 8056);
                    var message = Encoding.UTF8.GetBytes("START");
                    client.Send(message, message.Length);
                    while (_receive)
                    {
                        var receivedResult = await client.ReceiveAsync();
                        var receivedString = Encoding.UTF8.GetString(receivedResult.Buffer);
                        HandleData(receivedString);
                    }
                }
            });
        }

        private void HandleData(string data)
        {
            var vehicleData = VehicleData.FromJson(data);

            Speed = (int) vehicleData.Speed;
            OnPropertyChanged(nameof(Speed));
            
            UpdateSpeedometerNeedleAngle((int) vehicleData.Speed);
        }

        private void UpdateSpeedometerNeedleAngle(int speed)
        {
            var angle = speed <= 100
                ? MapValueToRange(speed, 0, 100, -145, 0) + 5
                : MapValueToRange(speed, 100, 300, 0, 145) - 5;
            
            SpeedometerNeedleRotation = angle;
            OnPropertyChanged(nameof(SpeedometerNeedleRotation));
        }

        private static int MapValueToRange(int value, int inputStart, int inputEnd, int outputStart, int outputEnd)
        {
            var inputRange = inputEnd - inputStart;
            var outputRange = outputEnd - outputStart;
            return (value - inputStart) * outputRange / inputRange + outputStart;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}