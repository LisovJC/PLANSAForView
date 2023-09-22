using System.Windows;
using System.Windows.Input;

namespace PLANSA
{  
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if(Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }
                this.DragMove();
            }    
                
        }
    }
}
