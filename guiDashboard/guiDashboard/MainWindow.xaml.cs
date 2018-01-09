using System;
using System.ComponentModel;
using System.Data.SQLite;
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
        public string Rpm { get; private set; }
        public int RpmNeedleRotation { get; private set; }
        public string Temperature { get; private set; }
        public string TimeOfDay { get; private set; }
        public string DrivenDistance { get; private set; }
        public int FuelDisplayWidth { get; private set; }
        public bool BlinkLeft { get; private set; }
        public bool BlinkRight { get; private set; }

        private bool _receive = true;

        public MainWindow()
        {
            InitializeComponent();
            FetchSqliteData();
            StartSimulationListener();
        }


        private static void FetchSqliteData()
        {
            // TODO do something with the data x)

            using (var database = new SQLiteConnection(@"Data Source=Resources\consumption_monitoring.db"))
            {
                database.Open();
                var consumptionForTrackOne =
                    new SQLiteCommand("SELECT consumption FROM Consumption WHERE trackid=1", database);
                var reader = consumptionForTrackOne.ExecuteReader();

                while (reader.Read())
                {
                    var consumption = float.Parse(reader["consumption"].ToString());
//                    Console.WriteLine(consumption);
                }
                database.Close();
            }
        }

        private void StartSimulationListener()
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
                        HandleSimulationData(receivedString);
                    }
                }
            });
        }

        private void HandleSimulationData(string data)
        {
            var vehicleData = VehicleData.FromJson(data);

            var speed = vehicleData.Speed;
            Speed = (int) speed;
            OnPropertyChanged(nameof(Speed));

            SpeedometerNeedleRotation = (int) (speed <= 100
                ? MapValueToRange(speed, 0, 100, -140, 0)
                : MapValueToRange(speed, 100, 300, 0, 138));
            OnPropertyChanged(nameof(SpeedometerNeedleRotation));

            var rpm = vehicleData.RPM / 1000f;
            Rpm = $"{rpm:0.0}";
            OnPropertyChanged(nameof(Rpm));

            RpmNeedleRotation = (int) MapValueToRange(rpm, 0, 8, -126, 128);
            OnPropertyChanged(nameof(RpmNeedleRotation));

            var temperature = vehicleData.OutsideTemperature;
            Temperature = $"{(temperature >= 0f ? "+" : "")}{temperature:0.0}°C";
            OnPropertyChanged(nameof(Temperature));

            var dayTime = DateTime.Now;
            TimeOfDay = dayTime.ToShortTimeString();
            OnPropertyChanged(nameof(TimeOfDay));

            var distance = vehicleData.Distance;
            DrivenDistance = $"{distance:0.0} km";
            OnPropertyChanged(nameof(DrivenDistance));

            var fuel = vehicleData.Fuel;
            FuelDisplayWidth = (int) MapValueToRange(fuel, 0f, 1f, 0, 159);
            OnPropertyChanged(nameof(FuelDisplayWidth));

            var blink = vehicleData.Blink;
            switch (blink)
            {
                case -1:
                    BlinkLeft = true;
                    BlinkRight = false;
                    break;
                case 1:
                    BlinkLeft = false;
                    BlinkRight = true;
                    break;
                default:
                    BlinkLeft = false;
                    BlinkRight = false;
                    break;
            }
            OnPropertyChanged(nameof(BlinkLeft));
            OnPropertyChanged(nameof(BlinkRight));
        }

        private static float MapValueToRange(float value, float inputStart, float inputEnd, float outputStart,
            float outputEnd)
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