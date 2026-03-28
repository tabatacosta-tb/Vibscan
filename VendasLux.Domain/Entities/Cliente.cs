using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendasLux.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public Cliente(string nome, string email)
        {
            Id = Guid.NewGuid(); // Gera um ID único automaticamente
            Nome = nome;
            Email = email;
        }

        // Método para atualizar os dados mantendo o encapsulamento
        public void Atualizar(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }
    }
}
