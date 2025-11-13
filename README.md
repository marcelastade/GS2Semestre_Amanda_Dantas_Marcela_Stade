# GS2Semestre_Amanda_Dantas_Marcela_Stade

# Estrutura do Projeto

```plaintext
GS2/
├── Controllers/
│   └── PromptController.cs
├── Domain/
│   └── Prompt.cs
├── Repository/
│   ├── IPromptRepository.cs
│   └── PromptRepository.cs
├── Service/
│   ├── ICacheService.cs
│   └── CacheService.cs
└── Program.cs
```

# 📦 Modelagem do Domínio (Branch: master)

Modelamos a classe principal e garantimos a conexão com o banco de dados via Dapper.

🧱 Classe Prompt

```plaintext
namespace GS2.Domain
{
    public class Prompt
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; }
        public string DescPrompt { get; set; }
        public DateTime DataPrompt { get; set; }
    }
}
```

💾 Conexão com Banco

A classe PromptRepository utiliza o Dapper e o MySqlConnector para realizar operações no banco de dados. A conexão é criada no arquivo Program.cs.

# ⚙️ Implementação do Core (Branch: core)

- Controller (PromptController): Responsável pelos endpoints da API. Permite cadastrar novos prompts e futuramente buscar ou atualizar versões.

- Camada de Serviço (Service): Foi criada a abstração ICacheService e a classe CacheService, que implementa cache em memória.

- Validações: Foram implementadas as validações utilizando try/catch.
