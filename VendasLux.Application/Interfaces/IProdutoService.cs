using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Application.DTOs;

namespace VendasLux.Application.Interfaces
{
    public interface IProdutoService
    {
        IEnumerable<ProdutoResponse> ObterTodos();
        ProdutoResponse ObterPorId(Guid id);
        ProdutoResponse Adicionar(ProdutoRequest request);
    }
}
