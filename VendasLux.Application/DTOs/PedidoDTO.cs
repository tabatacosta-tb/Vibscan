using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendasLux.Application.DTOs
{
    
    // recebe da API
    public record ItemPedidoRequest(Guid? ProdutoId, String? ProdutoNome,int Quantidade);
    public record PedidoRequest(Guid? ClienteId, string? ClienteNome, List<ItemPedidoRequest> Itens);

    // devolve para a API
    public record ItemPedidoResponse(Guid Id, Guid ProdutoId, int Quantidade, decimal PrecoUnitario, decimal Subtotal);
    public record PedidoResponse(Guid Id, Guid ClienteId, DateTime DataPedido, decimal ValorTotal, List<ItemPedidoResponse> Itens);
}
