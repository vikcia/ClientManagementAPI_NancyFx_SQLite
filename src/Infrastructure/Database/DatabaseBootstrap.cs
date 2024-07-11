using Dapper;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Database;

public class DatabaseBootstrap : IDatabaseBootstrap
{
    private readonly IDbConnection _connection;

    public DatabaseBootstrap(IDbConnection connection)
    {
        _connection = connection;
    }

    public void Setup()
    {
        CreateTableIfNotExists(
            "Clients",
            @"CREATE TABLE Clients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name VARCHAR(100) NOT NULL,
                    Age INT NULL,
                    Comment VARCHAR(1000) NULL);"
        );

        CreateTableIfNotExists(
        "Operation_history",
        @"CREATE TABLE Operation_history (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ClientId INTEGER NOT NULL,
                Status VARCHAR(100) NOT NULL,
                Date DATE NOT NULL);"
         );
    }

    private void CreateTableIfNotExists(string tableName, string createTableSql)
    {
        var table = _connection.Query<string>($"SELECT name FROM sqlite_master WHERE type='table' AND name = '{tableName}';");
        var existingTableName = table.FirstOrDefault();
        if (string.IsNullOrEmpty(existingTableName))
        {
            _connection.Execute(createTableSql);
        }
    }
}