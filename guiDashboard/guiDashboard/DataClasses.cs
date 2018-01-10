using System;
using System.Web.Script.Serialization;

namespace guiDashboard
{
    /// <summary>
    /// Data class to store Vehicle data (JSON format).
    /// The data source is an UDP client (driving simulator).
    /// </summary>
    public class VehicleData
    {
        public int Blink;
        public float Distance;
        public float Fuel;
        public bool FullBeam;
        public int Gear;
        public bool Light;
        public float OutsideTemperature;
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
    
    /// <summary>
    /// Data class to store Track data (from SQLite database).
    /// A list of TrackData will be provided as ItemsSource for the track selection DataGrid.
    /// </summary>
    public class TrackData
    {
        public string Id { get; }
        public string Start { get; }
        public string Destination { get; }
        public string Duration { get; }
        public string Date { get; }

        public TrackData(string id, string start, string destination, string duration, string date)
        {
            Id = id;
            Start = start;
            Destination = destination;
            Duration = duration;
            Date = date;
        }
    }
    
    /// <summary>
    /// Data class for storing Music tracks.
    /// A list of SongData will be provided for the song selection ListBox.
    /// </summary>
    public class SongData
    {
        public string Title { get; }
        public TimeSpan Duration { get; }

        public SongData(string title, TimeSpan duration)
        {
            Title = title;
            Duration = duration;
        }
    }
}