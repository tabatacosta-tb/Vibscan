namespace VibeScan.Domain.ValueObjects;

/// <summary>
/// Value Object que representa a categoria de um problema encontrado.
/// Imutável por design — dois objetos com mesmo valor são iguais.
/// </summary>
public sealed class CategoriaProblema : IEquatable<CategoriaProblema>
{
    public static readonly CategoriaProblema Seguranca     = new("Segurança");
    public static readonly CategoriaProblema Arquitetura   = new("Arquitetura");
    public static readonly CategoriaProblema BoasPraticas  = new("Boas Práticas");
    public static readonly CategoriaProblema Performance   = new("Performance");

    public string Valor { get; }

    private CategoriaProblema(string valor) => Valor = valor;

    public static CategoriaProblema From(string valor) =>
        valor switch
        {
            "Segurança"      => Seguranca,
            "Arquitetura"    => Arquitetura,
            "Boas Práticas"  => BoasPraticas,
            "Performance"    => Performance,
            _ => throw new ArgumentException($"Categoria inválida: {valor}")
        };

    public bool Equals(CategoriaProblema? other) => other is not null && Valor == other.Valor;
    public override bool Equals(object? obj) => obj is CategoriaProblema c && Equals(c);
    public override int GetHashCode() => Valor.GetHashCode();
    public override string ToString() => Valor;
}
