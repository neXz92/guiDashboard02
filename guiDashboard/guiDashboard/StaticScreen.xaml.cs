using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace guiDashboard
{
    public sealed partial class StaticScreen : INotifyPropertyChanged
    {
        public List<TrackData> Tracks { get; private set; } = new List<TrackData>();
        public string CurrentSong { get; private set; } = "Irgend ein Song";

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


        public StaticScreen()
        {
            InitializeComponent();
            InitializeTrackList();
            InitializeSongList();
        }

        private void InitializeTrackList()
        {
            using (var sqlite = new SQLiteConnection(@"Data Source=Resources\consumption_monitoring.db"))
            {
                sqlite.Open();

                const string sql = "SELECT id, startpoint, destination, durationinmin, trackdate FROM Track";
                var command = new SQLiteCommand(sql, sqlite);
                var reader = command.ExecuteReader();

                var values = new List<TrackData>();
                while (reader.Read())
                {
                    var id = reader["id"].ToString();
                    var start = reader["startpoint"].ToString();
                    var destination = reader["destination"].ToString();
                    var duration = reader["durationinmin"].ToString();
                    var date = reader["trackdate"].ToString();

                    values.Add(new TrackData(id, start, destination, duration, date));
                }
                Tracks = values;
                OnPropertyChanged(nameof(Tracks));

                sqlite.Close();
            }
            TrackTable.SelectedIndex = 0;
            UpdateConsumptionChart(1);
        }

        private void InitializeSongList()
        {
            //SongList.ItemsSource = Songs;
            SongList.SelectedIndex = 0;
        }

        private void OnTrackSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid?.SelectedItem is TrackData track)
                UpdateConsumptionChart(int.Parse(track.Id));
        }

        private void OnSongSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListBox;
            if (list?.SelectedItem is SongData song)
            {
                CurrentSong = $"{song.Title}";
                OnPropertyChanged(nameof(CurrentSong));
            }
        }

        private void UpdateConsumptionChart(int trackId)
        {
            using (var sqlite = new SQLiteConnection(@"Data Source=Resources\consumption_monitoring.db"))
            {
                sqlite.Open();

                var sql = $"SELECT time, consumption FROM Consumption WHERE trackid={trackId}";
                var command = new SQLiteCommand(sql, sqlite);
                var reader = command.ExecuteReader();

                var values = new List<KeyValuePair<DateTime, float>>();
                while (reader.Read())
                {
                    var time = DateTime.Parse(reader["time"].ToString());
                    var consumption = float.Parse(reader["consumption"].ToString());
                    values.Add(new KeyValuePair<DateTime, float>(time, consumption));
                }
                var averageConsumption = values.Average(kv => kv.Value);

                ConsumptionChart.Title = $"Strecke {trackId} — Ø {averageConsumption:0.0} l/100km";
                ConsumptionChart.DataContext = values;

                sqlite.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}