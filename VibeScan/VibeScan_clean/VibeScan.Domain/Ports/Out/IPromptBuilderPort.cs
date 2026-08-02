namespace VibeScan.Domain.Ports.Out;

/// <summary>
/// Port de saída — abstração para construção do prompt enviado à IA.
/// Separado por SRP: montar prompt é responsabilidade distinta de chamar a IA.
/// </summary>
public interface IPromptBuilderPort
{
    string Construir(string codigo, string? promptOriginal);
}
