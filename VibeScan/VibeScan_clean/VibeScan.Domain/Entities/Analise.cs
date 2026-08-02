using VibeScan.Domain.Exceptions;
using VibeScan.Domain.ValueObjects;

namespace VibeScan.Domain.Entities;

/// <summary>
/// Aggregate Root — representa uma análise completa de um código vibe coding.
/// Contém todas as regras de negócio e invariantes do domínio.
/// </summary>
public sealed class Analise
{
    private readonly List<ProblemaEncontrado> _problemas = new();

    public Guid Id { get; }
    public string CodigoOriginal { get; }
    public string? PromptOriginal { get; }
    public string ResumoExecutivo { get; private set; } = string.Empty;
    public ScoreQualidade? Score { get; private set; }
    public string ArquiteturaRecomendada { get; private set; } = string.Empty;
    public string PromptMelhorado { get; private set; } = string.Empty;
    public DateTime CriadoEm { get; }
    public StatusAnalise Status { get; private set; }

    public IReadOnlyList<ProblemaEncontrado> Problemas => _problemas.AsReadOnly();

    // Construtor privado para factory method
    private Analise(string codigoOriginal, string? promptOriginal)
    {
        Id              = Guid.NewGuid();
        CodigoOriginal  = codigoOriginal;
        PromptOriginal  = promptOriginal;
        CriadoEm       = DateTime.UtcNow;
        Status          = StatusAnalise.Pendente;
    }

    /// <summary>
    /// Factory Method — garante que uma Analise nunca seja criada inválida.
    /// </summary>
    public static Analise Criar(string codigoOriginal, string? promptOriginal = null)
    {
        if (string.IsNullOrWhiteSpace(codigoOriginal))
            throw new DomainException("O código a ser analisado não pode ser vazio.");

        if (codigoOriginal.Length > 50_000)
            throw new DomainException("O código não pode ultrapassar 50.000 caracteres.");

        return new Analise(codigoOriginal, promptOriginal);
    }

    /// <summary>
    /// Registra o resultado da análise realizada pelo adapter de IA.
    /// </summary>
    public void RegistrarResultado(
        string resumo,
        int score,
        string arquiteturaRecomendada,
        string promptMelhorado,
        IEnumerable<ProblemaEncontrado> problemas)
    {
        if (Status != StatusAnalise.Pendente)
            throw new DomainException("Resultado já foi registrado para esta análise.");

        ResumoExecutivo         = resumo;
        Score                   = new ScoreQualidade(score);
        ArquiteturaRecomendada  = arquiteturaRecomendada;
        PromptMelhorado         = promptMelhorado;
        Status                  = StatusAnalise.Concluida;

        _problemas.AddRange(problemas);
    }

    public void MarcarComoFalha() => Status = StatusAnalise.Falha;

    public int TotalProblemasCriticos =>
        _problemas.Count(p => p.NivelSeveridade == Severidade.Critico);
}

public enum StatusAnalise
{
    Pendente  = 0,
    Concluida = 1,
    Falha     = 2
}
