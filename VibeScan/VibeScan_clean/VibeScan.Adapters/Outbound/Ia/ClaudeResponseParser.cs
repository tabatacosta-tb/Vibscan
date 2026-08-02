using System.Text.Json;
using VibeScan.Domain.Ports.Out;

namespace VibeScan.Adapters.Outbound.Ia;

/// <summary>
/// Responsável exclusivamente por parsear e validar o JSON retornado pela IA.
/// SRP: ClaudeAnalisadorAdapter chama a API; este parser trata o retorno.
/// </summary>
public static class ClaudeResponseParser
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static ResultadoIa Parse(string jsonBruto)
    {
        // Remove blocos de markdown caso a IA os inclua mesmo sendo instruída a não
        var json = jsonBruto
            .Replace("```json", string.Empty)
            .Replace("```", string.Empty)
            .Trim();

        var doc = JsonSerializer.Deserialize<ClaudeResultadoJson>(json, _options)
            ?? throw new InvalidOperationException("A IA retornou um JSON nulo ou inválido.");

        var problemas = (doc.Problemas ?? [])
            .Select(p => new ProblemaIa(
                NormalizarCategoria(p.Categoria ?? "Boas Práticas"),
                p.Descricao ?? "Problema não descrito",
                p.Sugestao  ?? "Sem sugestão disponível",
                NormalizarSeveridade(p.Severidade ?? "Aviso")))
            .ToList();

        return new ResultadoIa(
            doc.Resumo                ?? "Sem resumo disponível.",
            Math.Clamp(doc.Score, 0, 10),
            doc.ArquiteturaRecomendada ?? "Não identificada",
            doc.PromptMelhorado        ?? "Não gerado",
            problemas);
    }

    private static string NormalizarCategoria(string raw) => raw.Trim() switch
    {
        var c when c.Contains("egur",  StringComparison.OrdinalIgnoreCase) => "Segurança",
        var c when c.Contains("rqui",  StringComparison.OrdinalIgnoreCase) => "Arquitetura",
        var c when c.Contains("erfor", StringComparison.OrdinalIgnoreCase) => "Performance",
        _ => "Boas Práticas"
    };

    private static string NormalizarSeveridade(string raw) => raw.Trim().ToLower() switch
    {
        var s when s.Contains("criti") => "Critico",
        var s when s.Contains("info")  => "Info",
        _ => "Aviso"
    };

    // Tipos internos para deserialização — não expostos fora do parser
    private sealed class ClaudeResultadoJson
    {
        public string?                    Resumo                 { get; init; }
        public int                        Score                  { get; init; }
        public string?                    ArquiteturaRecomendada { get; init; }
        public string?                    PromptMelhorado        { get; init; }
        public List<ProblemaJson>?        Problemas              { get; init; }
    }

    private sealed class ProblemaJson
    {
        public string? Categoria  { get; init; }
        public string? Descricao  { get; init; }
        public string? Sugestao   { get; init; }
        public string? Severidade { get; init; }
    }
}
