using System.Data.SQLite;
using System.Collections.Generic;
using System;

namespace Fast4Sale
{
    public class BD
    {
        private string connectionString = "Data Source=users.db;Version=3;";

        public BD()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT,
                        Email TEXT,
                        Password TEXT
                    )";

                new SQLiteCommand(sql, connection).ExecuteNonQuery();
            }
        }

        public void SaveUser(string username, string email, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Users (Username, Email, Password) VALUES (@u, @e, @p)";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@p", password);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetAllUsers()
        {
            var users = new List<string>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Username, Email, Password FROM Users";
                using (var cmd = new SQLiteCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string user = $"Логин: {reader["Username"]}, Email: {reader["Email"]}, Пароль: {reader["Password"]}";
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        // ПРОВЕРИТЬ пользователя (для входа)
        public bool CheckLogin(string usernameOrEmail, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Проверяем и по Username И по Email
                string sql = "SELECT 1 FROM Users WHERE (Username = @input OR Email = @input) AND Password = @p";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@input", usernameOrEmail);
                    cmd.Parameters.AddWithValue("@p", password);

                    var result = cmd.ExecuteScalar();
                    return result != null;
                }
            }
        }
        public bool DeleteUserById(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Users WHERE Id = @id";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public int GetID(string usernameOrEmail)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Id FROM Users WHERE Username = @input OR Email = @input";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@input", usernameOrEmail);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    return -1;
                }
            }
        }
        public string GetUsername(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Username FROM Users WHERE Id = @id";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }
                    return "эрор";
                }
            }
        }
    }
}