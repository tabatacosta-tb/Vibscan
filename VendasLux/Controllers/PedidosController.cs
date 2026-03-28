using Microsoft.AspNetCore.Mvc;
using VendasLux.Application.DTOs;
using VendasLux.Application.Interfaces;

namespace VendasLux.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public IActionResult CriarPedido([FromBody] PedidoRequest request)
        {
            try
            {
                var response = _pedidoService.CriarPedido(request);
                return Ok(response); 
            }
            catch (Exception ex)
            {
                
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            var pedidos = _pedidoService.ObterTodos();
            return Ok(pedidos);
        }
    }
}
