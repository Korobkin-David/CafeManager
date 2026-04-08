using System;
using System.Windows;
using System.Windows.Controls;
using MySqlConnector; 
using CafeManager.Database; 
using CafeManager.Utils;

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
        parentWindow?.MainFrame.Navigate(new Login());
    }

    private void roleInput_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.ContextMenu != null)
        {
            btn.ContextMenu.PlacementTarget = btn;
            btn.ContextMenu.IsOpen = true;
        }
    }

    public void RegisterButtonClick(object sender, RoutedEventArgs e)
    {
        string name = nameInput.Text.Trim();
        string surname = surnameInput.Text.Trim();
        string email = emailInput.Text.Trim();
        string password = passwordInput.Password.Trim();
        
        // Získání textu z obsahu tlačítka
        string role = roleInput.Content?.ToString(); 

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || 
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || 
            role == "Vyberte roli" || string.IsNullOrEmpty(role))
        {
            MessageBox.Show("Vyplňte všechny údaje a vyberte roli");
            return;
        }

        RegisterUser(name, surname, email, password, role); 
    }
    
    // Tato metoda musí být propojená v XAML (viz níže)
    private void RoleMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem item)
        {
            roleInput.Content = item.Header.ToString();
        }
    }

    public void RegisterUser(string name, string surname, string email, string password, string role)
    {
        // 1. Zahashování hesla pomocí tvého helperu
        string hashedPassword = CafeManager.Utils.PasswordHelper.HashPassword(password); 
    
        // 2. Vytvoření instance databáze (BEZ 'using', protože Database.cs ho nemá)
        var db = new CafeManager.Database.Database(); 
    
        // 3. Otevření připojení - zde už 'using' být MUSÍ
        using (var conn = db.GetConnection()) 
        {
            int roleId = 0;

            // 4. Zjištění ID role
            using (var cmd = new MySqlCommand("SELECT id FROM roles WHERE name=@role LIMIT 1", conn))
            {
                cmd.Parameters.AddWithValue("@role", role);
                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    MessageBox.Show("Vybraná role neexistuje!");
                    return;
                }
                roleId = Convert.ToInt32(result);
            }

            // 5. Vložení uživatele do tabulky
            using (var cmd = new MySqlCommand(
                       "INSERT INTO users (name, surname, email, passwordhash, role_id) VALUES (@name, @surname, @email, @pass, @roleId)", conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pass", hashedPassword);
                cmd.Parameters.AddWithValue("@roleId", roleId);

                try
                {
                    cmd.ExecuteNonQuery();
                    var parentWindow = Window.GetWindow(this) as MainWindow;
                    parentWindow?.MainFrame.Navigate(new Login());
                }
                catch (MySqlException ex)
                {
                    // Pokud email už existuje, MySQL vrátí chybu
                    MessageBox.Show("Chyba při registraci (možná již existující email): " + ex.Message);
                }
            }
        }
    }
}