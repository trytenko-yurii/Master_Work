using Master_Work.Connection;

namespace Master_Work.Repository
{
    public class GenericRepository
    {
        private readonly ConnectionFactory _cFactory;

        public GenericRepository(IConfiguration configuration)
        {
            // Получаем строку подключения из appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _cFactory = new ConnectionFactory(connectionString);
        }

        public async Task<List<Dictionary<string, object>>> GetUsersAsync()
        {
            string query = "SELECT * FROM Users";
            return await _cFactory.ExecuteQueryAsync(query);
        }

        public Task<int> AddUserAsync(string name, string email, string lastName, string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string query = $"INSERT INTO Users (Name, Email, Last_Name, Password) VALUES (@Name, @Email, @Last_Name, @Password)";
            var parameters = new Dictionary<string, object>
            {
                { "@Name", name },
                { "@Email", email },
                { "@Last_name", lastName },
                { "@Password", hashedPassword } 
            };

            return _cFactory.ExecuteNonQueryAsync(query, parameters);
        }
    }
}
