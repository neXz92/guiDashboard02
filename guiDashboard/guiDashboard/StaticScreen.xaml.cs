using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Controls;

namespace guiDashboard
{
    public sealed partial class StaticScreen
    {
        private readonly DataBindings _bindings;


        public StaticScreen()
        {
            InitializeComponent();
            _bindings = DataContext as DataBindings;
            
            InitializeTrackList();
            SongList.SelectedIndex = 0;
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
                _bindings.Tracks = values;
                sqlite.Close();
            }
            TrackTable.SelectedIndex = 0;
            UpdateConsumptionChart(1);
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
                _bindings.CurrentSong = $"{song.Title}";
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
    }
}