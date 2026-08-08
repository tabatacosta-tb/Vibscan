using VibeScan.Domain.Entities;
using VibeScan.Domain.Ports.In;
using VibeScan.Domain.Ports.Out;

namespace VibeScan.Application.UseCases;

/// <summary>
/// Use Case: consulta o histórico de análises realizadas.
/// </summary>
public sealed class ObterHistoricoUseCase : IObterHistoricoUseCase
{
    private readonly IAnaliseRepositoryPort _repositorio;

    public ObterHistoricoUseCase(IAnaliseRepositoryPort repositorio)
    {
        _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
    }

    public async Task<IReadOnlyList<Analise>> ExecutarAsync(CancellationToken ct = default)
        => await _repositorio.ListarTodosAsync(ct);

    public async Task<Analise?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => await _repositorio.ObterPorIdAsync(id, ct);
}
