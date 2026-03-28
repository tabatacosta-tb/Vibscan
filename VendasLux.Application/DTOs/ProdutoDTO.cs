using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendasLux.Application.DTOs
{
    public record ProdutoRequest(string Nome, decimal Preco);
    public record ProdutoResponse(Guid Id, string Nome, decimal Preco);
}
