using VibeScan.Domain.ValueObjects;

namespace VibeScan.Domain.Entities;

/// <summary>
/// Representa um problema específico encontrado na análise do código.
/// </summary>
public sealed class ProblemaEncontrado
{
    public Guid Id { get; }
    public CategoriaProblema Categoria { get; }
    public string Descricao { get; }
    public string Sugestao { get; }
    public Severidade NivelSeveridade { get; }

    public ProblemaEncontrado(
        CategoriaProblema categoria,
        string descricao,
        string sugestao,
        Severidade severidade)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição do problema não pode ser vazia.", nameof(descricao));

        if (string.IsNullOrWhiteSpace(sugestao))
            throw new ArgumentException("Sugestão não pode ser vazia.", nameof(sugestao));

        Id               = Guid.NewGuid();
        Categoria        = categoria ?? throw new ArgumentNullException(nameof(categoria));
        Descricao        = descricao;
        Sugestao         = sugestao;
        NivelSeveridade  = severidade;
    }
}

public enum Severidade
{
    Info    = 0,
    Aviso   = 1,
    Critico = 2
}
