using VibeScan.Domain.Exceptions;

namespace VibeScan.Domain.ValueObjects;

/// <summary>
/// Value Object que representa a nota de qualidade de um código analisado.
/// Garante invariante: score sempre entre 0 e 10.
/// </summary>
public sealed class ScoreQualidade : IEquatable<ScoreQualidade>
{
    public int Valor { get; }

    public ScoreQualidade(int valor)
    {
        if (valor < 0 || valor > 10)
            throw new DomainException($"Score de qualidade deve estar entre 0 e 10. Valor recebido: {valor}");

        Valor = valor;
    }

    public string Classificacao => Valor switch
    {
        >= 9 => "Excelente",
        >= 7 => "Bom",
        >= 5 => "Regular",
        >= 3 => "Ruim",
        _    => "Crítico"
    };

    public string Cor => Valor switch
    {
        >= 9 => "#22c55e",
        >= 7 => "#84cc16",
        >= 5 => "#eab308",
        >= 3 => "#f97316",
        _    => "#ef4444"
    };

    public bool Equals(ScoreQualidade? other) => other is not null && Valor == other.Valor;
    public override bool Equals(object? obj) => obj is ScoreQualidade s && Equals(s);
    public override int GetHashCode() => Valor.GetHashCode();
    public override string ToString() => $"{Valor}/10 — {Classificacao}";
}
