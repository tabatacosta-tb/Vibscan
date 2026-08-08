using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibeScan.Adapters.Inbound.Http.Mappers;
using VibeScan.Application.DTOs;
using VibeScan.Domain.Ports.In;

namespace VibeScan.Adapters.Inbound.Http.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class HistoricoController : ControllerBase
{
    private readonly IObterHistoricoUseCase _useCase;

    public HistoricoController(IObterHistoricoUseCase useCase)
    {
        _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
    }

    /// <summary>
    /// Lista todas as análises realizadas na sessão atual.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<HistoricoItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAsync(CancellationToken ct)
    {
        var analises = await _useCase.ExecutarAsync(ct);
        var dtos = analises.Select(AnaliseMapper.ToHistoricoItem).ToList();
        return Ok(dtos);
    }

    /// <summary>
    /// Obtém o detalhe completo de uma análise pelo Id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AnalisarCodigoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorIdAsync(Guid id, CancellationToken ct)
    {
        var analise = await _useCase.ObterPorIdAsync(id, ct);

        if (analise is null)
            return NotFound(new { erro = $"Análise '{id}' não encontrada." });

        return Ok(AnaliseMapper.ToResponse(analise));
    }
}
