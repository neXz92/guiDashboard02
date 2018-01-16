using System.Windows;
using System.Windows.Input;

namespace guiDashboard
{
    public partial class SecondWindow
    {
        public SecondWindow()
        {
            InitializeComponent();
        }

        private void OnLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Top = (SystemParameters.WorkArea.Height - Height) / 2;
            Left = (SystemParameters.WorkArea.Width - Width) / 2;
        }
    }
}
