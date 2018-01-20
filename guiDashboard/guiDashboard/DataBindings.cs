using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace guiDashboard
{
    public class DataBindings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int Speed { get; set; }
        public int SpeedometerNeedleRotation { get; set; }
        public string Rpm { get; set; }
        public int RpmNeedleRotation { get; set; }
        public string Temperature { get; set; }
        public string TimeOfDay { get; set; }
        public string DrivenDistance { get; set; }
        public int FuelDisplayWidth { get; set; }
        public bool BlinkLeft { get; set; }
        public bool BlinkRight { get; set; }
        public bool FullBeam { get; set; }
        public bool LowBeam { get; set; }
        public bool Light { get; set; }
        public bool FogLight { get; set; }
        public bool WarnSignal { get; set; }
        public bool Wiper { get; set; }
        public bool OilSignal { get; set; }
        public bool BatterySignal { get; set; }
        public bool BrakeSignal { get; set; }
        public bool DoorSignal { get; set; }
        public bool DefrostSignal { get; set; }
        
        public List<TrackData> Tracks { get; set; } = new List<TrackData>();
        public IEnumerable<SongData> Songs { get; } = new List<SongData>
        {
            new SongData("Globus - A Thousand Deaths", new TimeSpan(0, 6, 30)),
            new SongData("Billy Talent - Red Flag", new TimeSpan(0, 3, 16)),
            new SongData("Avril Lavigne - Sk8er Boi", new TimeSpan(0, 3, 24)),
            new SongData("System of A Down - Toxicity", new TimeSpan(0, 3, 39)),
            new SongData("Within Temptation - Let Us Burn", new TimeSpan(0, 5, 31)),
            new SongData("Linkin Park - In the End", new TimeSpan(0, 3, 36)),
            new SongData("Avantasia - Journey to Arcadia", new TimeSpan(0, 7, 12))
        };
        public string CurrentSong { get; set; }
    }
}