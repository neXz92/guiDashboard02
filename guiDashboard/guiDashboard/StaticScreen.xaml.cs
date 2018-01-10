using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace guiDashboard
{
    /// <summary>
    /// Public data class to store Track data (from SQLite database)
    /// A list of TrackData will be provided as ItemsSource for the track selection DataGrid
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

    public sealed partial class StaticScreen : INotifyPropertyChanged
    {
        public List<TrackData> Tracks { get; private set; } = new List<TrackData>();


        public StaticScreen()
        {
            InitializeComponent();
            InitializeTrackList();

            TrackTable.SelectedIndex = 0;
            UpdateConsumptionChart(1);
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
        }

        private void OnTrackSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;

            if (grid?.SelectedItem is TrackData track)
                UpdateConsumptionChart(int.Parse(track.Id));
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