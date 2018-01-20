using System;
using System.Windows;

namespace guiDashboard
{
    public partial class App
    {
        private Dashboard _dashboard;
        

        private void OnExit(object sender, ExitEventArgs e)
        {
            _dashboard.ShutDown();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _dashboard = new Dashboard();
            _dashboard.Show();

            MainWindow = _dashboard;
        }
    }
}
