using System.Windows;
using System.Windows.Controls;
using MySqlConnector; // Sjednoceno na MySqlConnector
using CafeManager.Database; // Přidán přístup k DB
using CafeManager.Utils; // Přidán přístup k PasswordHelperu

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

    // NOVÁ METODA: Obsluha přihlášení
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
            // Hledáme uživatele podle emailu
            using (var cmd = new MySqlCommand("SELECT passwordhash FROM users WHERE email = @email", conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                var storedHash = cmd.ExecuteScalar()?.ToString();
                // pokud se porovná heslo s zahashovaným heslem tak to (buď dá error nebo pustí na další stránku)
                if (storedHash != null && VerifyPassword(rawPassword, storedHash))
                {
                    var parentWindow = Window.GetWindow(this) as MainWindow;
                    parentWindow?.MainFrame.Navigate(new Menu());
                }
                else
                {
                    MessageBox.Show("Neplatný email nebo heslo.");
                }
            }
        }
    }

    // Pomocná metoda pro ověření (PasswordHelper)
    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        return PasswordHelper.VerifyPassword(inputPassword, storedHash);
    }
}