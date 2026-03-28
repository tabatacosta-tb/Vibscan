using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendasLux.Domain.Entities
{
    public class ItemPedido
    {
        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; } // Guarda o preço na hora da compra

        public ItemPedido(Guid produtoId, int quantidade, decimal precoUnitario)
        {
            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        // Regra de negócio para calcular o subtotal deste item
        public decimal CalcularSubtotal()
        {
            return Quantidade * PrecoUnitario;
        }
    }
}
