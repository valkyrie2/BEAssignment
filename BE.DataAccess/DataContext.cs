using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BE.DataAccess
{
    public class DataContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(Configuration.GetConnectionString("UserDatabase"));
        }

        public async Task Init()
        {
            // create database tables if they don't exist
            using var connection = CreateConnection();
            await _initUsers();

            async Task _initUsers()
            {
                var sql = """
                CREATE TABLE IF NOT EXISTS 
                Users (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Email TEXT
                );
            """;
                await connection.ExecuteAsync(sql);
            }
        }
    }
}