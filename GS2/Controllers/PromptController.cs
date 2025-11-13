using GS2.Domain;
using GS2.Repository;
using GS2.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GS2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromptController : ControllerBase
    {
        private const string cacheKey = "prompts-cache";
        private readonly IPromptRepository _promptRepository;
        private readonly ICacheService cacheService;
        private readonly ILogger<PromptController> logger;

        public PromptController(IPromptRepository promptRepository, ICacheService cacheService, ILogger<PromptController> logger)
        {
            this._promptRepository = promptRepository;
            this.cacheService = cacheService;
            this.logger = logger;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> AddPrompt([FromBody] Prompt prompt)
        {
            if (prompt == null)
                return BadRequest("Prompt inválido.");

            try
            {
                prompt.DataPrompt = DateTime.Now;
                int id = await _promptRepository.AddPromptAsync(prompt);
                return Ok(new { Message = "Prompt cadastrado com sucesso!", Id = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar prompt: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromptById(int id)
        {
            try
            {
                var prompt = await _promptRepository.GetPromptByIdAsync(id);
                if (prompt == null)
                    return NotFound("Prompt não encontrado.");

                return Ok(prompt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar prompt: {ex.Message}");
            }
        }
        [HttpGet("allPrompts")]
        public async Task<IActionResult> GetAllPrompts()
        {
            try
            {
                var prompt = await _promptRepository.GetAllPromptAsync();
                if (prompt == null)
                    return NotFound("Não há prompts registrados.");

                return Ok(prompt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar prompts: {ex.Message}");
            }
        }


        [HttpPut("{id}/prompt")]
        public async Task<IActionResult> AtualizarPrompt(int id, [FromQuery] string novoPrompt)
        {
            try
            {
                await _promptRepository.AtualizarPromptAsync(id, novoPrompt);
                return Ok(new { Message = $"Descrição do prompt {id} atualizada para {novoPrompt}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar prompt: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return BadRequest("Prompt inválido.");

            try
            {
                await _promptRepository.DeletePromptAsync(id);
                return Ok(new { Message = $"Prompt excluído com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir prompt: {ex.Message}");
            }
        }

        private async Task InvalidateCache()
        {
            try
            {
                await cacheService.DeleteAsync(cacheKey);
                logger.LogInformation("Cache invalidado com sucesso");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Erro ao invalidar cache Redis, mas operação continuará");
            }
        }
    }

}
