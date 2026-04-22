using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
using CafeManager.Database;
using CafeManager.Utils;

namespace CafeManager.Pages;

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
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow?.MainFrame.Navigate(new Register());
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        string email = emailInput.Text.Trim();
        string rawPassword = password.Password.Trim();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(rawPassword))
        {
            MessageBox.Show("Zadejte email a heslo.");
            return;
        }

        var db = new CafeManager.Database.Database();
        using (var conn = db.GetConnection())
        {
            using (var cmd = new MySqlCommand(
                       "SELECT id, passwordhash, name, role_id FROM users WHERE email = @email", conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedHash = reader.GetString("passwordhash");
                    if (VerifyPassword(rawPassword, storedHash))
                    {
                        SessionManager.UserId   = reader.GetInt32("id");
                        SessionManager.UserName = reader.GetString("name");
                        SessionManager.UserRole = reader.GetInt32("role_id");

                        var parentWindow = Window.GetWindow(this) as MainWindow;
                        parentWindow?.MainFrame.Navigate(new Menu());
                    }
                    else
                    {
                        MessageBox.Show("Neplatný email nebo heslo.");
                    }
                }
                else
                {
                    MessageBox.Show("Neplatný email nebo heslo.");
                }
            }
        }
    }

    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        return PasswordHelper.VerifyPassword(inputPassword, storedHash);
    }
}