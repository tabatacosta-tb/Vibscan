using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Domain.Entities;

namespace VendasLux.Domain.Interfaces
{
    public interface IClienteRepository
    {
        IEnumerable<Cliente> ObterTodos();
        Cliente ObterPorId(Guid id);
        Cliente ObterPorNome(string nome);
        void Adicionar(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Remover(Guid id);
    }
}
