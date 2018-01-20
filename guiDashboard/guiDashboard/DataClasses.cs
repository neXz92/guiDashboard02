using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

// ReSharper disable All

namespace guiDashboard
{
    /// <summary>
    /// Data class to store Vehicle data (JSON format).
    /// The data source is an UDP client (driving simulator).
    /// Note that not all the possible data is used in our dashboard design.
    /// </summary>
    public class VehicleData
    {
        public string[] AdditionalParameters;
        public int Blink;
        public int CoolantTemperature;
        public float Distance;
        public int ErrorCode;
        public string ErrorMessage;
        public bool FogLamp;
        public float Fuel;
        public bool FullBeam;
        public int Gear;
        public bool Light;
        public KeyValuePair<string, string>[] Notifications;
        public float OilLevel;
        public float OilPressure;
        public float OutsideTemperature;
        public float RPM;
        public float Speed;
        public float[] TirePressure;
        public bool WarnsignalCoolantTemperature;
        public bool WarnsignalOilLevel;
        public bool WarnsignalOilPressure;
        public bool WarnsignalTirePressure;
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