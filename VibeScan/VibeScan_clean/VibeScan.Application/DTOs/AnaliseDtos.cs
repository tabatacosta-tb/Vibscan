using VibeScan.Domain.Entities;
using VibeScan.Domain.ValueObjects;

namespace VibeScan.Application.DTOs;

// ── Request ─────────────────────────────────────────────────────────────────

public record AnalisarCodigoRequest(
    string CodigoOriginal,
    string? PromptOriginal
);

// ── Response ─────────────────────────────────────────────────────────────────

public record AnalisarCodigoResponse(
    Guid Id,
    string ResumoExecutivo,
    ScoreDto Score,
    string ArquiteturaRecomendada,
    string PromptMelhorado,
    IReadOnlyList<ProblemaDto> Problemas,
    DateTime CriadoEm
);

public record ScoreDto(
    int Valor,
    string Classificacao,
    string Cor
);

public record ProblemaDto(
    string Categoria,
    string Descricao,
    string Sugestao,
    string Severidade
);

public record HistoricoItemDto(
    Guid Id,
    int? Score,
    string? Classificacao,
    string? Cor,
    int TotalProblemas,
    int ProblemasCriticos,
    DateTime CriadoEm,
    StatusAnalise Status
);
