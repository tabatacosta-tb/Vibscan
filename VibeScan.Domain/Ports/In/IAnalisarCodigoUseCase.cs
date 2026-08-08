using VibeScan.Domain.Entities;

namespace VibeScan.Domain.Ports.In;

/// <summary>
/// Port de entrada — contrato que o mundo externo usa para disparar uma análise.
/// Driven Adapters implementam; Use Cases consomem.
/// </summary>
public interface IAnalisarCodigoUseCase
{
    Task<Analise> ExecutarAsync(string codigoOriginal, string? promptOriginal, CancellationToken ct = default);
}
