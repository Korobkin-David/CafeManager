using MySqlConnector;
using System;
using System.IO;

namespace CafeManager.Database
{
    public class Database
    {
        private const string Server = "Server=localhost;Port=3306;User=root;Password=;";
        private const string WithDb = "Server=localhost;Port=3306;Database=CafeManager;User=root;Password=;";

        public void Initialize()
        {
            CreateDatabaseIfNotExists();
            RunSchema();
        }

        private void CreateDatabaseIfNotExists()
        {
            using var connection = new MySqlConnection(Server);
            connection.Open();
            using var cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS CafeManager;", connection);
            cmd.ExecuteNonQuery();
        }

        private void RunSchema()
        {
            string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "schema.sql");
            if (!File.Exists(schemaPath))
                throw new FileNotFoundException($"Soubor schema.sql nenalezen: {schemaPath}");

            string sql = File.ReadAllText(schemaPath);

            using var connection = new MySqlConnection(WithDb);
            connection.Open();

            foreach (var part in sql.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmed = part.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;
                if (trimmed.StartsWith("CREATE DATABASE", StringComparison.OrdinalIgnoreCase)) continue;
                if (trimmed.StartsWith("USE ", StringComparison.OrdinalIgnoreCase)) continue;

                using var cmd = new MySqlCommand(trimmed, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(WithDb);
            conn.Open();
            return conn;
        }
    }
}