using VibeScan.Domain.Entities;

namespace VibeScan.Domain.Ports.Out;

/// <summary>
/// Port de saída — abstração para qualquer provedor de IA.
/// O domínio nunca sabe se é Claude, OpenAI ou qualquer outro.
/// Princípio OCP: novo provedor = novo adapter, zero mudança no domínio.
/// </summary>
public interface IAnalisadorPort
{
    Task<ResultadoIa> AnalisarAsync(string codigo, string? promptOriginal, CancellationToken ct = default);
}

/// <summary>
/// DTO de retorno da IA — estrutura neutra, sem acoplamento a nenhum provider.
/// </summary>
public record ResultadoIa(
    string Resumo,
    int Score,
    string ArquiteturaRecomendada,
    string PromptMelhorado,
    IReadOnlyList<ProblemaIa> Problemas
);

public record ProblemaIa(
    string Categoria,
    string Descricao,
    string Sugestao,
    string Severidade
);
