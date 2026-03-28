using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Domain.Entities;

namespace VendasLux.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        IEnumerable<Pedido> ObterTodos();
        Pedido ObterPorId(Guid id);
        void Adicionar(Pedido pedido);
    }
}
