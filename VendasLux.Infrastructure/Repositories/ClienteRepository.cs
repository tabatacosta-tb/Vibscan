using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Domain.Entities;
using VendasLux.Domain.Interfaces;

namespace VendasLux.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        // Esta lista estática simula tabela de Clientes no banco de dados
        private static readonly List<Cliente> _clientes = new();

        public void Adicionar(Cliente cliente)
        {
            _clientes.Add(cliente);
        }

        public Cliente ObterPorId(Guid id)
        {
            return _clientes.FirstOrDefault(c => c.Id == id);
        }

        public Cliente ObterPorNome(string nome)
        {           
            return _clientes.FirstOrDefault(c => c.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Cliente> ObterTodos()
        {
            return _clientes;
        }

        public void Atualizar(Cliente cliente)
        {
            var clienteExistente = ObterPorId(cliente.Id);
            if (clienteExistente != null)
            {
                clienteExistente.Atualizar(cliente.Nome, cliente.Email);
            }
        }

        public void Remover(Guid id)
        {
            var cliente = ObterPorId(id);
            if (cliente != null)
            {
                _clientes.Remove(cliente);
            }
        }
    }
}
