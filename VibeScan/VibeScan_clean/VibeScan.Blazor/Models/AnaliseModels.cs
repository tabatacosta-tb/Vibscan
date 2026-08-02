namespace VibeScan.Blazor.Models;

public record AnalisarRequest(string CodigoOriginal, string? PromptOriginal);

public record AnalisarResponse(
    Guid Id,
    string ResumoExecutivo,
    ScoreModel Score,
    string ArquiteturaRecomendada,
    string PromptMelhorado,
    List<ProblemaModel> Problemas,
    DateTime CriadoEm
);

public record ScoreModel(int Valor, string Classificacao, string Cor);

public record ProblemaModel(
    string Categoria,
    string Descricao,
    string Sugestao,
    string Severidade
);

public record HistoricoItem(
    Guid Id,
    int? Score,
    string? Classificacao,
    string? Cor,
    int TotalProblemas,
    int ProblemasCriticos,
    DateTime CriadoEm,
    string Status
);
