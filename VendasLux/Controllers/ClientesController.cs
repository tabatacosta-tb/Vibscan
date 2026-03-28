using Microsoft.AspNetCore.Mvc;
using VendasLux.Application.DTOs;
using VendasLux.Application.Interfaces;

namespace VendasLux.API.Controllers
{
    [ApiController] // Diz que esta classe é uma API
    [Route("api/[controller]")] // A rota será: /api/clientes
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        // Injetando o serviço pelo construtor (Inversão de Dependência)
        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost] // Requisição para CRIAR
        public IActionResult Adicionar([FromBody] ClienteRequest request)
        {
            var response = _clienteService.Adicionar(request);

            // Retorna o Status 201 (Created) e aponta para a rota de busca
            return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
        }

        [HttpGet] // Requisição para LISTAR TODOS
        public IActionResult ObterTodos()
        {
            var clientes = _clienteService.ObterTodos();
            return Ok(clientes); // Status 200 com a lista
        }

        [HttpGet("{id:guid}")] // Requisição para BUSCAR POR ID
        public IActionResult ObterPorId(Guid id)
        {
            var cliente = _clienteService.ObterPorId(id);

            if (cliente == null)
                return NotFound("Cliente não encontrado."); 

            return Ok(cliente); 
        }
    }
}
