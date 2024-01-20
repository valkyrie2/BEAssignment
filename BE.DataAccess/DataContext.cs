using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BE.DataAccess
{
    public abstract class DataContextBase
    {
        public DataContextBase()
        {
        }

        public abstract IDbConnection CreateConnection();

        public abstract Task InitAsync();
    }

    public class DataContext : DataContextBase
    {
        protected readonly IConfiguration Configuration;
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override IDbConnection CreateConnection()
        {
            return new SqliteConnection(Configuration.GetConnectionString("UserDatabase"));
        }

        public override async Task InitAsync()
        {
            // create database tables if they don't exist
            using var connection = CreateConnection();
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