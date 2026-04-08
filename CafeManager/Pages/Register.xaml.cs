using System.Windows;
using System.Windows.Controls;

namespace CafeManager.Pages;

public partial class Register : Page
{
    public Register()
    {
        InitializeComponent();
    }
    private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.MainFrame.Navigate(new Login());
    }
    private void roleInput_Click(object sender, RoutedEventArgs e)
    {
        Button btn = sender as Button;

        if (btn.ContextMenu != null)
        {
            btn.ContextMenu.PlacementTarget = btn;
            btn.ContextMenu.IsOpen = true;
        }
    }

    public void Registration()
    {
        string nameInput;
        string surnameInput;
        string emailInput;
        string passwordInput;
        string roleInput;
    }
}