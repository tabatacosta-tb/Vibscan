using VibeScan.Domain.Entities;

namespace VibeScan.Domain.Ports.In;

/// <summary>
/// Port de entrada — contrato para consulta do histórico de análises.
/// </summary>
public interface IObterHistoricoUseCase
{
    Task<IReadOnlyList<Analise>> ExecutarAsync(CancellationToken ct = default);
    Task<Analise?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
}
