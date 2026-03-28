using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendasLux.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public DateTime DataPedido { get; private set; }

        // (Encapsulamento)
        private readonly List<ItemPedido> _itens;

        public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

        public Pedido(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;           
            DataPedido = DateTime.Now;
            _itens = new List<ItemPedido>();
        }

        // Método para adicionar itens ao pedido
        public void AdicionarItem(ItemPedido item)
        {
            _itens.Add(item);
        }

        // Regra de negócio Pedido sabe calcular seu próprio valor total
        public decimal CalcularValorTotal()
        {
            return _itens.Sum(item => item.CalcularSubtotal());
        }
    }
}
