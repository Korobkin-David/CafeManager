using System.Windows;
using System.Windows.Controls;

namespace CafeManager.Pages
{
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            this.Loaded += Login_Loaded;
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Maximized;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Převezmi Frame z rodičovského okna a naviguj na Register
            var parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.MainFrame.Navigate(new Register());
        }
    }
}