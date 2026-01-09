using System.Data.SQLite;
using System.Collections.Generic;
using System;


public class Advertisement
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public string Area { get; set; }
    public string Rooms { get; set; }
    public string Floor { get; set; }
    public string TotalFloors { get; set; }
    public string Price { get; set; }
    public string Type { get; set; }
    public DateTime CreatedDate { get; set; }
    public byte[] PhotoData { get; set; }
}


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

                // Таблица пользователей
                string sql = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        PhoneNumber TEXT NOT NULL,
                        Password TEXT NOT NULL
                    )";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();

                string sqlAds = @"
                    CREATE TABLE IF NOT EXISTS Advertisements (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER,
                        Title TEXT NOT NULL,
                        Address TEXT NOT NULL,
                        Description TEXT,
                        Area TEXT,
                        Rooms TEXT,
                        Floor TEXT,
                        TotalFloors TEXT,
                        Price TEXT NOT NULL,
                        Type TEXT NOT NULL,
                        PhotoData BLOB,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    )";
                new SQLiteCommand(sqlAds, connection).ExecuteNonQuery();
            }
        }

        public (string Username, string Email, string PhoneNumber) GetUserContacts(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Username, Email, PhoneNumber FROM Users WHERE Id = @id";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (
                                reader["Username"].ToString(),
                                reader["Email"].ToString(),
                                reader["PhoneNumber"].ToString()
                            );
                        }
                    }
                }
            }

            return (string.Empty, string.Empty, string.Empty);
        }


        public void SaveUser(string username, string email, string PhoneNumber, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Users (Username, Email, PhoneNumber, Password) VALUES (@u, @e, @num, @p)";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@num", PhoneNumber);
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

        public bool CheckLogin(string usernameOrEmail, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

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

                string sqlDeleteAds = "DELETE FROM Advertisements WHERE UserId = @id";
                using (var cmdAds = new SQLiteCommand(sqlDeleteAds, connection))
                {
                    cmdAds.Parameters.AddWithValue("@id", userId);
                    cmdAds.ExecuteNonQuery();
                }

                string sqlDeleteUser = "DELETE FROM Users WHERE Id = @id";
                using (var cmd = new SQLiteCommand(sqlDeleteUser, connection))
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
                    return "Пользователь не найден";
                }
            }
        }


        public int SaveAdvertisement(int userId, string title, string address, string description,
                                    string area, string rooms, string floor, string totalFloors, string price,
                                    string type, byte[] photo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    INSERT INTO Advertisements 
                    (UserId, Title, Address, Description, Area, Rooms, Floor, TotalFloors, Price, Type, PhotoData) 
                    VALUES (@userId, @title, @address, @description, @area, @rooms, @floor, @totalFloors, @price, @type, @photo);
                    SELECT last_insert_rowid();";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@rooms", rooms);
                    cmd.Parameters.AddWithValue("@floor", floor);
                    cmd.Parameters.AddWithValue("@totalFloors", totalFloors);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@photo", photo);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public List<Advertisement> GetAllAdvertisements()
        {
            var ads = new List<Advertisement>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT a.*, u.Username 
                    FROM Advertisements a
                    LEFT JOIN Users u ON a.UserId = u.Id
                    ORDER BY a.CreatedDate DESC";

                using (var cmd = new SQLiteCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ads.Add(new Advertisement
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            UserId = Convert.ToInt32(reader["UserId"]),
                            Username = reader["Username"].ToString(),
                            Title = reader["Title"].ToString(),
                            Address = reader["Address"].ToString(),
                            Description = reader["Description"].ToString(),
                            Area = reader["Area"].ToString(),
                            Rooms = reader["Rooms"].ToString(),
                            Floor = reader["Floor"].ToString(),
                            TotalFloors = reader["TotalFloors"].ToString(),
                            Price = reader["Price"].ToString(),
                            Type = reader["Type"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            PhotoData = reader["PhotoData"] as byte[]
                        });
                    }
                }
            }

            return ads;
        }

        public List<Advertisement> GetUserAdvertisements(int userId)
        {
            var ads = new List<Advertisement>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Advertisements WHERE UserId = @userId ORDER BY CreatedDate DESC";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ads.Add(new Advertisement
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Title = reader["Title"].ToString(),
                                Address = reader["Address"].ToString(),
                                Price = reader["Price"].ToString(),
                                Type = reader["Type"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                PhotoData = reader["PhotoData"] as byte[]
                            });
                        }
                    }
                }
            }

            return ads;
        }

        public bool DeleteAdvertisement(int adId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Advertisements WHERE Id = @id";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", adId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public Advertisement GetAdvertisementById(int adId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT a.*, u.Username 
                    FROM Advertisements a
                    LEFT JOIN Users u ON a.UserId = u.Id
                    WHERE a.Id = @adId";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@adId", adId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var ad = new Advertisement
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                Title = reader["Title"].ToString(),
                                Address = reader["Address"].ToString(),
                                Description = reader["Description"].ToString(),
                                Area = reader["Area"].ToString(),
                                Rooms = reader["Rooms"].ToString(),
                                Floor = reader["Floor"].ToString(),
                                TotalFloors = reader["TotalFloors"].ToString(),
                                Price = reader["Price"].ToString(),
                                Type = reader["Type"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                PhotoData = reader["PhotoData"] as byte[]
                            };

                            return ad;
                        }
                    }
                }
            }

            return null;
        }

        public bool UpdateAdvertisement(int adId, string title, string address, string description,
                                string area, string rooms, string floor, string totalFloors, string price,
                                string type, byte[] photo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    UPDATE Advertisements
                    SET
                        Title = @title,
                        Address = @address,
                        Description = @description,
                        Area = @area,
                        Rooms = @rooms,
                        Floor = @floor,
                        TotalFloors = @totalFloors,
                        Price = @price,
                        Type = @type,
                        PhotoData = COALESCE(@photo, PhotoData)
                    WHERE Id = @adId";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@adId", adId);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@rooms", rooms);
                    cmd.Parameters.AddWithValue("@floor", floor);
                    cmd.Parameters.AddWithValue("@totalFloors", totalFloors);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@photo", photo ?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public List<Advertisement> SearchAdvertisements(string searchTerm, string minPrice, string maxPrice, string type)
        {
            var ads = new List<Advertisement>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT a.*, u.Username 
                    FROM Advertisements a
                    LEFT JOIN Users u ON a.UserId = u.Id
                    WHERE 1=1";

                var parameters = new List<SQLiteParameter>();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sql += " AND (a.Title LIKE @search OR a.Description LIKE @search OR a.Address LIKE @search)";
                    parameters.Add(new SQLiteParameter("@search", $"%{searchTerm}%"));
                }

                if (!string.IsNullOrEmpty(minPrice))
                {
                    sql += " AND CAST(a.Price AS INTEGER) >= @minPrice";
                    parameters.Add(new SQLiteParameter("@minPrice", minPrice));
                }

                if (!string.IsNullOrEmpty(maxPrice))
                {
                    sql += " AND CAST(a.Price AS INTEGER) <= @maxPrice";
                    parameters.Add(new SQLiteParameter("@maxPrice", maxPrice));
                }

                if (!string.IsNullOrEmpty(type) && type != "Все")
                {
                    sql += " AND a.Type = @type";
                    parameters.Add(new SQLiteParameter("@type", type));
                }

                sql += " ORDER BY a.CreatedDate DESC";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ads.Add(new Advertisement
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                Title = reader["Title"].ToString(),
                                Address = reader["Address"].ToString(),
                                Description = reader["Description"].ToString(),
                                Area = reader["Area"].ToString(),
                                Rooms = reader["Rooms"].ToString(),
                                Floor = reader["Floor"].ToString(),
                                TotalFloors = reader["TotalFloors"].ToString(),
                                Price = reader["Price"].ToString(),
                                Type = reader["Type"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                PhotoData = reader["PhotoData"] as byte[]
                            });
                        }
                    }
                }
            }

            return ads;
        }
    }
}