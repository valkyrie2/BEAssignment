using BE.DataAccess;
using BE.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace BE.Tests
{
    public class UserTests : IClassFixture<DataContextFixture>
    {
        private readonly SqliteConnection _connection;

        public UserTests(DataContextFixture fixture)
        {
            _connection = fixture.DB;
        }

        [Fact]
        public async Task InsertTest()
        {
            const string sql = "INSERT INTO Users (Name,Email) VALUES (@Name, @Email)";
            var affected = await _connection.ExecuteAsync(sql,
                new[] { new User { Name = "GHI", Email = "d@d.d" }, new User { Name = "JKL", Email = "e@e.e" } });

            Assert.Equal(2, affected);
        }
        [Fact]
        public async Task UpdateTest()
        {
            const string sql = "UPDATE Users SET Email=@Email WHERE Id=@Id";
            var affected = await _connection.ExecuteAsync(sql, new{ Id = 1, Email = "z@z.z" });

            Assert.Equal(1, affected);
        }

        [Fact]
        public async Task DeleteTest()
        {
            const string sql = "DELETE FROM Users WHERE Id=@Id";
            var del = await _connection.ExecuteAsync(sql, new { Id = 2 });
            var affected = await _connection.QueryAsync<User>("SELECT * FROM Users WHERE Id=@Id", new { Id = 2 });

            Assert.Null(affected.FirstOrDefault());
        }

        [Fact]
        public async Task UserbyIdTest()
        {
            const string sql = "SELECT * FROM Users WHERE Id=@Id";
            var user = await _connection.QueryAsync<User>(sql, new { Id = 1 });

            Assert.NotNull(user.FirstOrDefault());
        }

        [Fact]
        public async Task UserbyNameTest()
        {
            const string sql = "SELECT COUNT(*) FROM Users WHERE Name LIKE '%' || @Name || '%'";
            var affected = await _connection.QuerySingleAsync<int>(sql, new { Name = 'B' });

            Assert.True(affected > 1);
        }
    }

    public class DataContextFixture : DataContextBase, IDisposable
    {
        public DataContextFixture()
        {
            DB = new SqliteConnection("DataSource=file::memory:?cache=shared");
            InitAsync().Wait();
        }

        public override IDbConnection CreateConnection()
        {
            return new SqliteConnection("DataSource=file::memory:?cache=shared");
        }

        public async void Dispose()
        {
            await ResetAsync();
        }

        public SqliteConnection DB { get; private set; }

        public override async Task InitAsync()
        {
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

            const string insertSQL = "INSERT INTO Users (Name,Email) VALUES (@Name, @Email)";
            await connection.ExecuteAsync(insertSQL,
                new[] { new User { Name = "Hello", Email = "a@a.a" }, new User { Name = "ABC", Email = "b@b.b" }, new User { Name = "DEF", Email = "c@c.c" } });
        }

        public async Task ResetAsync()
        {
            using var connection = CreateConnection();

            await connection.ExecuteAsync("DELETE FROM Users");
            await connection.ExecuteAsync("DELETE FROM SQLITE_SEQUENCE WHERE name='Users';");

            const string insertSQL = "INSERT INTO Users (Name,Email) VALUES (@Name, @Email)";
            await connection.ExecuteAsync(insertSQL,
                new[] { new User{ Name = "Hello", Email = "a@a.a" }, new User{ Name = "ABC", Email = "b@b.b" }, new User{ Name = "BCD", Email = "c@c.c" } });
        }
    }
}