using Dapper;
using Microsoft.Data.Sqlite;
using BE.Model;

namespace BE.DataAccess
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetList();
        Task<User> GetByUsername(string name);
        Task<User> GetById(int id);
        Task Add(User user);
        Task Update(User user);
    }
    public class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetList()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<User>("SELECT * FROM Users");
        }
        public async Task<User> GetByUsername(string name)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Name = @name", new { Name = name });
        }

        public async Task<User> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
        }

        public async Task Add(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = """
                INSERT INTO Users (Name, Email) 
                VALUES (@Name, @Email)
             """;
            var result = await connection.ExecuteAsync(sql, user);
        }

        public async Task Update(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = """
                UPDATE Users 
                SET Name = @Name,
                    Email = @Email
                WHERE Id = @Id
             """;
            await connection.ExecuteAsync(sql, user);
        }
    }
}