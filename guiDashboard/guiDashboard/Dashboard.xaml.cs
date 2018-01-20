using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace guiDashboard
{
    public sealed partial class Dashboard
    {
        private readonly DataBindings _bindings;
        private readonly UdpClient _client;

        private readonly Queue<float> _speedValues = new Queue<float>();
        private readonly Queue<float> _rpmValues = new Queue<float>();


        public Dashboard()
        {
            InitializeComponent();
            _bindings = DataContext as DataBindings;
            _client = new UdpClient();

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
                var startMessage = Encoding.UTF8.GetBytes("START");
                using (_client)
                {
                    _client.Connect("localhost", 8056);
                    _client.Send(startMessage, startMessage.Length);
                    while (true)
                    {
                        var receivedResult = await _client.ReceiveAsync();
                        var receivedString = Encoding.UTF8.GetString(receivedResult.Buffer);
                        HandleSimulationData(receivedString);
                    }
                }
            });
        }

        private void HandleSimulationData(string data)
        {
            var vehicleData = VehicleData.FromJson(data);

            _speedValues.Enqueue(vehicleData.Speed);
            if (_speedValues.Count > 5)
                _speedValues.Dequeue();

            var speed = _speedValues.Average();
            _bindings.Speed = (int) speed;
            _bindings.SpeedometerNeedleRotation = (int) (speed <= 100
                ? MapValueToRange(speed, 0, 100, -140, 0)
                : MapValueToRange(speed, 100, 300, 0, 138));

            _rpmValues.Enqueue(vehicleData.RPM);
            if (_rpmValues.Count > 5)
                _rpmValues.Dequeue();

            var rpm = _rpmValues.Average() / 1000f;
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
            _bindings.FogLight = vehicleData.FogLamp;
            _bindings.OilSignal = vehicleData.WarnsignalOilLevel;

            // Misuse some of the other warn signals for our custom icons
            _bindings.BatterySignal = vehicleData.WarnsignalOilPressure;
            _bindings.DoorSignal = vehicleData.WarnsignalTirePressure;
            _bindings.DefrostSignal = vehicleData.WarnsignalCoolantTemperature;
            _bindings.BrakeSignal = vehicleData.ErrorCode != 0;
        }

        private static float MapValueToRange(float value,
            float inputStart, float inputEnd,
            float outputStart, float outputEnd)
        {
            var inputRange = inputEnd - inputStart;
            var outputRange = outputEnd - outputStart;
            return (value - inputStart) * outputRange / inputRange + outputStart;
        }

        public void ShutDown()
        {
            var stopMessage = Encoding.UTF8.GetBytes("STOP");
            _client.Send(stopMessage, stopMessage.Length);
            _client.Close();
        }
    }
}