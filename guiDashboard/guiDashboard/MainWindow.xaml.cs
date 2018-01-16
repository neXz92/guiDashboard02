using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace guiDashboard
{
    public sealed partial class MainWindow
    {
        private readonly DataBindings _bindings;
        private bool _receive = true;


        public MainWindow()
        {
            InitializeComponent();
            _bindings = DataContext as DataBindings;

            var second = new SecondWindow();
            second.Show();

            AnimateCurrentSongText();
            StartSimulationListener();
        }

        private void AnimateCurrentSongText()
        {
            var animation = new DoubleAnimation
            {
                From = CurrentSongBounds.Width,
                To = -CurrentSongLabel.Width,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(8))
            };
            CurrentSongLabel.BeginAnimation(Canvas.LeftProperty, animation);
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
            _bindings.Speed = (int) speed;
            _bindings.SpeedometerNeedleRotation = (int) (speed <= 100
                ? MapValueToRange(speed, 0, 100, -140, 0)
                : MapValueToRange(speed, 100, 300, 0, 138));

            var rpm = vehicleData.RPM / 1000f;
            _bindings.Rpm = $"{rpm:0.0}";
            _bindings.RpmNeedleRotation = (int) MapValueToRange(rpm, 0, 8, -126, 128);

            var temperature = vehicleData.OutsideTemperature;
            _bindings.Temperature = $"{(temperature >= 0f ? "+" : "")}{temperature:0.0}°C";

            var dayTime = DateTime.Now;
            _bindings.TimeOfDay = dayTime.ToShortTimeString();

            var distance = vehicleData.Distance;
            _bindings.DrivenDistance = $"{distance:0.0} km";

            var fuel = vehicleData.Fuel;
            _bindings.FuelDisplayWidth = (int) MapValueToRange(fuel, 0f, 1f, 0, 159);

            var blink = vehicleData.Blink;
            switch (blink)
            {
                case -1:
                    _bindings.BlinkLeft = true;
                    _bindings.BlinkRight = false;
                    break;
                case 1:
                    _bindings.BlinkLeft = false;
                    _bindings.BlinkRight = true;
                    break;
                default:
                    _bindings.BlinkLeft = false;
                    _bindings.BlinkRight = false;
                    break;
            }

            _bindings.Wiper = vehicleData.WiperLevel != 0;
            _bindings.FullBeam = vehicleData.FullBeam;
            _bindings.WarnSignal = vehicleData.Warnsignal;
            _bindings.Light = vehicleData.Light;
        }

        private static float MapValueToRange(float value,
            float inputStart, float inputEnd,
            float outputStart, float outputEnd)
        {
            var inputRange = inputEnd - inputStart;
            var outputRange = outputEnd - outputStart;
            return (value - inputStart) * outputRange / inputRange + outputStart;
        }
    }
}