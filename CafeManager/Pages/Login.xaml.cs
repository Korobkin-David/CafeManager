using System.Windows;
using System.Windows.Controls;

namespace CafeManager.Pages;

public partial class Login : Page
{
    public Login()
    {
        InitializeComponent();
    }
    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        // Převezmi Frame z rodičovského okna a naviguj na Register
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.MainFrame.Navigate(new Register());
    }
}