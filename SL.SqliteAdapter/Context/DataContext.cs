using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.SqliteAdapter.Context;

public class DataContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public async Task Init()
    {
        // create database tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();

        async Task _initUsers()
        {
            const string createUsers = @"
                CREATE TABLE IF NOT EXISTS 
                    Users (
                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        Email TEXT,
                        Password TEXT
            )";
            await connection.ExecuteAsync(createUsers);

            const string createLists = @"
                CREATE TABLE IF NOT EXISTS Lists (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Owner_id INTEGER,
                    FOREIGN KEY(owner_id) REFERENCES Users(id)
                );
            ";
            await connection.ExecuteAsync(createLists);

            const string createShare = @"
                CREATE TABLE IF NOT EXISTS Share (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    List_id INTEGER,
                    User_id INTEGER,
                    FOREIGN KEY(List_id) REFERENCES Lists(Id),
                    FOREIGN KEY(User_id) REFERENCES Users(Id)
                );
            ";
            await connection.ExecuteAsync(createShare);
        }
    }
}