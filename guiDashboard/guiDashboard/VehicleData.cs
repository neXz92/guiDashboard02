using System.Web.Script.Serialization;

namespace guiDashboard
{
    public class VehicleData
    {
        public int Blink;
        public float Distance;
        public float Fuel;
        public bool FullBeam;
        public int Gear;
        public bool Light;
        public int OutsideTemperature;
        public float RPM;
        public float Speed;
        public bool Warnsignal;
        public int WiperLevel;
        
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public static VehicleData FromJson(string json)
        {
            return Serializer.Deserialize<VehicleData>(json);
        }
    }
}