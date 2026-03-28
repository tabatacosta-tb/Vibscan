using Microsoft.AspNetCore.Mvc;
using VendasLux.Application.DTOs;
using VendasLux.Application.Interfaces;

namespace VendasLux.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] ProdutoRequest request)
        {
            var response = _produtoService.Adicionar(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            var produtos = _produtoService.ObterTodos();
            return Ok(produtos);
        }

        [HttpGet("{id:guid}")]
        public IActionResult ObterPorId(Guid id)
        {
            var produto = _produtoService.ObterPorId(id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            return Ok(produto);
        }
    }
}
