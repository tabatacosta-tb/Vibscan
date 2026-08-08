using VibeScan.Application.DTOs;
using VibeScan.Domain.Entities;

namespace VibeScan.Adapters.Inbound.Http.Mappers;

/// <summary>
/// Mapper responsável por converter entidades de domínio em DTOs de resposta HTTP.
/// Isola a camada de apresentação do domínio — o controller nunca acessa
/// propriedades internas do aggregate diretamente.
/// </summary>
public static class AnaliseMapper
{
    public static AnalisarCodigoResponse ToResponse(Analise analise) =>
        new(
            Id:                    analise.Id,
            ResumoExecutivo:       analise.ResumoExecutivo,
            Score:                 analise.Score is not null
                                        ? new ScoreDto(
                                            analise.Score.Valor,
                                            analise.Score.Classificacao,
                                            analise.Score.Cor)
                                        : new ScoreDto(0, "Não calculado", "#6b7280"),
            ArquiteturaRecomendada: analise.ArquiteturaRecomendada,
            PromptMelhorado:       analise.PromptMelhorado,
            Problemas:             analise.Problemas
                                        .Select(p => new ProblemaDto(
                                            p.Categoria.Valor,
                                            p.Descricao,
                                            p.Sugestao,
                                            p.NivelSeveridade.ToString()))
                                        .ToList()
                                        .AsReadOnly(),
            CriadoEm:              analise.CriadoEm
        );

    public static HistoricoItemDto ToHistoricoItem(Analise analise) =>
        new(
            Id:                 analise.Id,
            Score:              analise.Score?.Valor,
            Classificacao:      analise.Score?.Classificacao,
            Cor:                analise.Score?.Cor,
            TotalProblemas:     analise.Problemas.Count,
            ProblemasCriticos:  analise.TotalProblemasCriticos,
            CriadoEm:           analise.CriadoEm,
            Status:             analise.Status
        );
}
