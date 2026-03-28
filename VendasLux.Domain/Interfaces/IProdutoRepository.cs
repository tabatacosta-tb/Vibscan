using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Domain.Entities;

namespace VendasLux.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> ObterTodos();
        Produto ObterPorId(Guid id);
        Produto ObterPorNome(string nome);
        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
        void Remover(Guid id);
    }
}
