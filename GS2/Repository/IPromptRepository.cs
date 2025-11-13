using GS2.Domain;
using System.Threading.Tasks;

namespace GS2.Repository
{
    public interface IPromptRepository
    {
        Task<int> AddPromptAsync(Prompt prompt);
        Task<Prompt?> GetPromptByIdAsync(int id);
        Task<IEnumerable<Prompt?>> GetAllPromptAsync();
        Task AtualizarPromptAsync(int id, string novoPrompt);
        Task DeletePromptAsync(int id);
    }
}
