# VibeScan 🔍

**Avaliador de código Vibe Coding com diagnóstico arquitetural via IA**

Aplicação desenvolvida como Projeto Final de Pós-Graduação em Arquitetura de Software e Solução — Faculdade XPE.

---

## 🏗️ Arquitetura

```
VibeScan.Domain      → Entidades, Value Objects, Ports (interfaces)
VibeScan.Application → Use Cases (AnalisarCodigo, ObterHistorico)
VibeScan.Adapters    → Adapters HTTP, Claude IA, MemoryCache
VibeScan.API         → Entry point ASP.NET Core + DI
VibeScan.Blazor      → Frontend WebAssembly
```

Padrão: **Hexagonal Architecture (Ports & Adapters)**
Princípios: **SOLID · Clean Code · Design Patterns (Adapter, Repository, Factory, DTO)**

---

## 🚀 Como rodar

### Pré-requisitos
- .NET 8 SDK
- Chave de API Claude (https://console.anthropic.com)

### 1. Configurar a API Key

Edite `VibeScan.API/appsettings.json`:

```json
{
  "Claude": {
    "ApiKey": "sk-ant-sua-chave-aqui"
  }
}
```

> ⚠️ **Nunca commite sua ApiKey.** Em produção, use variável de ambiente:
> `Claude__ApiKey=sk-ant-...`

### 2. Rodar a API

```bash
cd VibeScan.API
dotnet run
# API disponível em https://localhost:7100
# Swagger em https://localhost:7100/swagger
```

### 3. Rodar o Blazor

```bash
cd VibeScan.Blazor
dotnet run
# Frontend em https://localhost:7200
```

---

## 📋 Endpoints da API

| Método | Rota                   | Descrição                          |
|--------|------------------------|------------------------------------|
| POST   | /api/analise           | Analisa um código vibe coding      |
| GET    | /api/historico         | Lista análises da sessão           |
| GET    | /api/historico/{id}    | Detalhe de uma análise específica  |

### Exemplo de requisição

```json
POST /api/analise
{
  "codigoOriginal": "var conn = new SqlConnection(\"Server=prod;Password=123\"); ...",
  "promptOriginal": "crie uma API que salva usuários no banco"
}
```

---

## 📦 Estrutura de Pastas

```
VibeScan/
├── VibeScan.Domain/
│   ├── Entities/         Analise.cs, ProblemaEncontrado.cs
│   ├── ValueObjects/     ScoreQualidade.cs, CategoriaProblema.cs
│   ├── Ports/In/         IAnalisarCodigoUseCase.cs, IObterHistoricoUseCase.cs
│   ├── Ports/Out/        IAnalisadorPort.cs, IAnaliseRepositoryPort.cs, IPromptBuilderPort.cs
│   └── Exceptions/       DomainException.cs
│
├── VibeScan.Application/
│   ├── UseCases/         AnalisarCodigoUseCase.cs, ObterHistoricoUseCase.cs
│   └── DTOs/             AnaliseDtos.cs
│
├── VibeScan.Adapters/
│   ├── Inbound/Http/     AnaliseController.cs, HistoricoController.cs, AnaliseMapper.cs
│   └── Outbound/
│       ├── Ia/           ClaudeAnalisadorAdapter.cs, ClaudeResponseParser.cs, ClaudeSettings.cs
│       ├── Cache/        MemoryCacheRepositoryAdapter.cs
│       └── Prompt/       PromptBuilderAdapter.cs
│
├── VibeScan.API/
│   ├── Program.cs
│   ├── appsettings.json
│   └── DependencyInjection/  ServiceCollectionExtensions.cs
│
└── VibeScan.Blazor/
    ├── Pages/            Index.razor, Historico.razor, Detalhe.razor
    ├── Components/       MainLayout.razor, ResultadoAnalise.razor
    ├── Services/         VibeScanApiService.cs
    ├── Models/           AnaliseModels.cs
    └── wwwroot/          index.html, css/app.css
```

---

## 🔄 Fluxo de uma análise

```
[Blazor] → POST /api/analise
  → AnaliseController
  → AnalisarCodigoUseCase
  → Analise.Criar()          (invariantes de domínio validadas)
  → IAnalisadorPort
  → ClaudeAnalisadorAdapter
  → PromptBuilderAdapter     (monta prompt estruturado)
  → Claude API               (retorna JSON)
  → ClaudeResponseParser     (parseia e normaliza)
  → analise.RegistrarResultado()
  → IAnaliseRepositoryPort
  → MemoryCacheRepositoryAdapter (persiste em memória)
  → AnaliseMapper.ToResponse()
  → [Blazor] exibe resultado
```

---

## 🔮 Próximos passos (Sprint 2 e 3)

- [ ] Score visual com gráfico de radar por categoria
- [ ] Exportar relatório em PDF/DOCX
- [ ] Histórico persistente (trocar MemoryCache por SQL Server sem tocar no domínio)
- [ ] Autenticação básica
- [ ] Testes unitários dos Use Cases e do PromptBuilder
