using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VibeScan.Adapters.Inbound.Http.Mappers;
using VibeScan.Application.DTOs;
using VibeScan.Domain.Exceptions;
using VibeScan.Domain.Ports.In;

namespace VibeScan.Adapters.Inbound.Http.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class AnaliseController : ControllerBase
{
    private readonly IAnalisarCodigoUseCase _useCase;
    private readonly ILogger<AnaliseController> _logger;

    public AnaliseController(
        IAnalisarCodigoUseCase useCase,
        ILogger<AnaliseController> logger)
    {
        _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        _logger  = logger  ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Analisa um código vibe coding e retorna diagnóstico completo.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AnalisarCodigoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AnalisarAsync(
        [FromBody] AnalisarCodigoRequest request,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CodigoOriginal))
            return BadRequest(new { erro = "O campo 'codigoOriginal' é obrigatório." });

        try
        {
            _logger.LogInformation("Iniciando análise de código. Tamanho: {Tamanho} chars",
                request.CodigoOriginal.Length);

            var analise = await _useCase.ExecutarAsync(
                request.CodigoOriginal,
                request.PromptOriginal,
                ct);

            _logger.LogInformation("Análise concluída. Id: {Id} | Score: {Score}",
                analise.Id, analise.Score?.Valor);

            return Ok(AnaliseMapper.ToResponse(analise));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Violação de regra de domínio: {Mensagem}", ex.Message);
            return BadRequest(new { erro = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Falha na comunicação com a API de IA.");
            return StatusCode(502, new { erro = "Falha na comunicação com o serviço de IA. Tente novamente." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado durante análise.");
            return StatusCode(500, new { erro = "Erro interno. Consulte os logs para detalhes." });
        }
    }
}
