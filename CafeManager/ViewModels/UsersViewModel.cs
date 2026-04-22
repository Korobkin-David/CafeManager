using DB = CafeManager.Database.Database;
using MySqlConnector;
using System.ComponentModel;
using CafeManager.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CafeManager.ViewModels;

public class UsersViewModel : INotifyPropertyChanged
{
    private readonly DB _db = new DB();

    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string _surname;

    public string Surname
    {
        get => _surname;
        set
        {
            _surname = value;
            OnPropertyChanged(nameof(Surname));
        }
    }

    private int _roleId;
    public int RoleId
    {
        get => _roleId;
        set
        {
            _roleId = value;
            OnPropertyChanged(nameof(RoleId));
        }
    }

    public void NactiUzivatele(int userId)
    {
        using var conn = _db.GetConnection();
        var cmd = new MySqlCommand(
            "SELECT name, surname, role_id FROM users WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", SessionManager.UserId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            Name = reader.GetString("name");
            Surname = reader.GetString("surname");
            RoleId = reader.GetInt32("role_id");
        }
    }

    public string Fullname => $"{Name} {Surname}";

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string prop) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}