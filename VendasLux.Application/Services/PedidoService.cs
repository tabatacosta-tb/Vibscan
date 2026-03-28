using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Application.DTOs;
using VendasLux.Application.Interfaces;
using VendasLux.Domain.Entities;
using VendasLux.Domain.Interfaces;

namespace VendasLux.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;

        // Injetamos os três repositórios para validar se cliente e produto existem antes do pedido
        public PedidoService(
            IPedidoRepository pedidoRepository,
            IClienteRepository clienteRepository,
            IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
        }

        public PedidoResponse CriarPedido(PedidoRequest request)
        {
            // encontrar o Cliente (Por ID ou Por Nome)
            Cliente cliente = null;

            if (request.ClienteId.HasValue)
            {
                cliente = _clienteRepository.ObterPorId(request.ClienteId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(request.ClienteNome))
            {
                cliente = _clienteRepository.ObterPorNome(request.ClienteNome);
            }

            if (cliente == null)
                throw new Exception("Cliente não encontrado. Informe um ClienteId ou ClienteNome válido.");

            // Cria o Pedido vazio 
            var pedido = new Pedido(cliente.Id);

            // encontrar cada Produto
            foreach (var itemRequest in request.Itens)
            {
                Produto produto = null;

                if (itemRequest.ProdutoId.HasValue)
                {
                    produto = _produtoRepository.ObterPorId(itemRequest.ProdutoId.Value);
                }
                else if (!string.IsNullOrWhiteSpace(itemRequest.ProdutoNome))
                {
                    produto = _produtoRepository.ObterPorNome(itemRequest.ProdutoNome);
                }

                if (produto == null)
                    throw new Exception($"Produto não encontrado (ID: {itemRequest.ProdutoId} / Nome: {itemRequest.ProdutoNome}).");

                var itemPedido = new ItemPedido(produto.Id, itemRequest.Quantidade, produto.Preco);
                pedido.AdicionarItem(itemPedido);
            }

            // Salva no repositório
            _pedidoRepository.Adicionar(pedido);

            // Mapeia para a Resposta (DTO)
            var itensResponse = pedido.Itens.Select(i => new ItemPedidoResponse(
                i.Id, i.ProdutoId, i.Quantidade, i.PrecoUnitario, i.CalcularSubtotal())).ToList();

            return new PedidoResponse(pedido.Id, pedido.ClienteId, pedido.DataPedido, pedido.CalcularValorTotal(), itensResponse);
        }

        public IEnumerable<PedidoResponse> ObterTodos()
        {
            var pedidos = _pedidoRepository.ObterTodos();
            return pedidos.Select(p => new PedidoResponse(p.Id,
                p.ClienteId,
                p.DataPedido,
                p.CalcularValorTotal(),
                p.Itens.Select(i => new ItemPedidoResponse(i.Id, i.ProdutoId, i.Quantidade, i.PrecoUnitario, i.CalcularSubtotal())).ToList()
            ));
        }
    }
}
