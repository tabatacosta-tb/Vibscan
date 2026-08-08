using VibeScan.Domain.Entities;
using VibeScan.Domain.Ports.In;
using VibeScan.Domain.Ports.Out;

namespace VibeScan.Application.UseCases;

/// <summary>
/// Use Case: orquestra o fluxo completo de análise de um código vibe coding.
/// Depende apenas de abstrações (ports) — nunca de implementações concretas.
/// Princípio DIP aplicado: alto nível não depende de baixo nível.
/// </summary>
public sealed class AnalisarCodigoUseCase : IAnalisarCodigoUseCase
{
    private readonly IAnalisadorPort _analisador;
    private readonly IAnaliseRepositoryPort _repositorio;

    public AnalisarCodigoUseCase(
        IAnalisadorPort analisador,
        IAnaliseRepositoryPort repositorio)
    {
        _analisador  = analisador  ?? throw new ArgumentNullException(nameof(analisador));
        _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
    }

    public async Task<Analise> ExecutarAsync(
        string codigoOriginal,
        string? promptOriginal,
        CancellationToken ct = default)
    {
        // 1. Criar aggregate — valida invariantes de domínio
        var analise = Analise.Criar(codigoOriginal, promptOriginal);

        try
        {
            // 2. Chamar port de saída (adapter de IA cuida dos detalhes)
            var resultado = await _analisador.AnalisarAsync(codigoOriginal, promptOriginal, ct);

            // 3. Mapear problemas da IA para entidades de domínio
            var problemas = resultado.Problemas
                .Select(p => new ProblemaEncontrado(
                    Domain.ValueObjects.CategoriaProblema.From(p.Categoria),
                    p.Descricao,
                    p.Sugestao,
                    Enum.Parse<Severidade>(p.Severidade, ignoreCase: true)))
                .ToList();

            // 4. Registrar resultado no aggregate (regras de domínio aplicadas aqui)
            analise.RegistrarResultado(
                resultado.Resumo,
                resultado.Score,
                resultado.ArquiteturaRecomendada,
                resultado.PromptMelhorado,
                problemas);
        }
        catch
        {
            analise.MarcarComoFalha();
            throw;
        }
        finally
        {
            // 5. Persistir independente do resultado (para histórico de falhas também)
            await _repositorio.SalvarAsync(analise, ct);
        }

        return analise;
    }
}
