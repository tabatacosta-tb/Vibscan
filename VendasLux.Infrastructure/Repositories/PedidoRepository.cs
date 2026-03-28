using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Domain.Entities;
using VendasLux.Domain.Interfaces;

namespace VendasLux.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        // Simulando a tabela de Pedidos
        private static readonly List<Pedido> _pedidos = new();

        public void Adicionar(Pedido pedido)
        {
            _pedidos.Add(pedido);
        }

        public Pedido ObterPorId(Guid id)
        {
            return _pedidos.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Pedido> ObterTodos()
        {
            return _pedidos;
        }
    }
}
