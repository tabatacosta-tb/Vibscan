using Microsoft.Extensions.Caching.Memory;
using VibeScan.Domain.Entities;
using VibeScan.Domain.Ports.Out;

namespace VibeScan.Adapters.Outbound.Cache;

/// <summary>
/// Adapter de persistência usando IMemoryCache (.NET built-in).
/// Implementa IAnaliseRepositoryPort — substitua por SqlServerRepositoryAdapter
/// na Sprint 2/3 sem tocar em uma linha do domínio ou dos use cases.
/// </summary>
public sealed class MemoryCacheRepositoryAdapter : IAnaliseRepositoryPort
{
    private const string CacheKey = "vibescan_historico";

    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public MemoryCacheRepositoryAdapter(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));

        // Histórico persiste por 8 horas em memória
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(8));
    }

    public Task SalvarAsync(Analise analise, CancellationToken ct = default)
    {
        var lista = ObterLista();
        lista.Add(analise);
        _cache.Set(CacheKey, lista, _cacheOptions);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Analise>> ListarTodosAsync(CancellationToken ct = default)
    {
        var lista = ObterLista();
        // Ordena por mais recente primeiro
        IReadOnlyList<Analise> resultado = lista
            .OrderByDescending(a => a.CriadoEm)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(resultado);
    }

    public Task<Analise?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        var analise = ObterLista().FirstOrDefault(a => a.Id == id);
        return Task.FromResult(analise);
    }

    private List<Analise> ObterLista()
        => _cache.GetOrCreate(CacheKey, entry =>
        {
            entry.SetOptions(_cacheOptions);
            return new List<Analise>();
        }) ?? new List<Analise>();
}
