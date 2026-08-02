using VibeScan.Domain.Entities;

namespace VibeScan.Domain.Ports.Out;

/// <summary>
/// Port de saída — abstração de persistência.
/// Hoje é cache em memória. Amanhã pode ser SQL Server, Redis, etc.
/// O domínio não sabe e não precisa saber.
/// </summary>
public interface IAnaliseRepositoryPort
{
    Task SalvarAsync(Analise analise, CancellationToken ct = default);
    Task<IReadOnlyList<Analise>> ListarTodosAsync(CancellationToken ct = default);
    Task<Analise?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
}
