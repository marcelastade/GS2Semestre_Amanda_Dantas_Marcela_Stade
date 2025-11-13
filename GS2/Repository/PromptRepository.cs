using Dapper;
using GS2.Domain;
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

        public async Task<int> AddPromptAsync(Prompt prompt)
        {
            if (prompt == null)
                throw new ArgumentNullException(nameof(prompt), "Prompt inválido.");

            if(prompt.NomeUsuario == null || string.IsNullOrWhiteSpace(prompt.NomeUsuario)) 
                throw new ArgumentNullException(nameof(prompt), "O nome do usuário é obrigatório.");
            if (prompt.DescPrompt == null || string.IsNullOrWhiteSpace(prompt.DescPrompt))
                throw new ArgumentNullException(nameof(prompt), "A descrição do prompt é obrigatória.");

            await _connection.OpenAsync();
            string sql = @"
                INSERT INTO Prompt (Id, NomeUsuario, DescPrompt, DataPrompt)
                VALUES (@Id, @NomeUsuario, @DescPrompt, @DataPrompt);
                SELECT LAST_INSERT_ID();
            ";
            var id = await _connection.ExecuteScalarAsync<int>(sql, prompt);
            await _connection.CloseAsync();
            return id;
        }

        public async Task<Prompt?> GetPromptByIdAsync(int id)
        {
            await _connection.OpenAsync();

            string sql = "SELECT * FROM Prompt WHERE Id = @id;";
            var prompt = await _connection.QueryFirstOrDefaultAsync<Prompt>(sql, new { id });
            await _connection.CloseAsync();
            return prompt;
        }

        public async Task <IEnumerable<Prompt?>> GetAllPromptAsync()
        {
            await _connection.OpenAsync();

            string sql = "SELECT * FROM Prompt;";
            var prompts = await _connection.QueryAsync<Prompt>(sql);
            await _connection.CloseAsync();
            return prompts;
        }

        public async Task AtualizarPromptAsync(int id, string novoPrompt)
        {
            await _connection.OpenAsync();

            string sql = "UPDATE Prompt SET DescPrompt = @novoPrompt WHERE Id = @id;";
            await _connection.ExecuteAsync(sql, new { id, novoPrompt });
            await _connection.CloseAsync();
        }
        public async Task DeletePromptAsync(int id)
        {
            var promptExiste = await GetPromptByIdAsync(id);
            if (id <= 0)
                throw new ArgumentException("ID inválido.", nameof(id));
            if (promptExiste == null)
                throw new ArgumentException("ID não existe.", nameof(id));
            await _connection.OpenAsync();
            string sql = "DELETE FROM Prompt WHERE id = @Id;";
            await _connection.ExecuteAsync(sql, new { Id = id });
            await _connection.CloseAsync();
        }
    }

}
