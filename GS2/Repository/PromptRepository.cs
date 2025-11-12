using MySqlConnector;

namespace GS2.Repository
{
    public class PromptRepository : IPromptRepository
    {
        private readonly MySqlConnection _connection;
        public PromptRepository(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }
    }
}
