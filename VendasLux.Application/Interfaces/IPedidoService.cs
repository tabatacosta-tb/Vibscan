using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Application.DTOs;

namespace VendasLux.Application.Interfaces
{
    public interface IPedidoService
    {
        IEnumerable<PedidoResponse> ObterTodos();
        PedidoResponse CriarPedido(PedidoRequest request);
    }
}
