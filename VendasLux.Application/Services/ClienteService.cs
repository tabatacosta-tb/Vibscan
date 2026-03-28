using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Application.DTOs;
using VendasLux.Application.Interfaces;
using VendasLux.Domain.Entities;
using VendasLux.Domain.Interfaces;

namespace VendasLux.Application.Services
{
           public class ClienteService : IClienteService
        {
            private readonly IClienteRepository _clienteRepository;

            // Injeção de Dependência
            public ClienteService(IClienteRepository clienteRepository)
            {
                _clienteRepository = clienteRepository;
            }

            public ClienteResponse Adicionar(ClienteRequest request)
            {
                // Transforma o DTO na Entidade de Domínio
                var cliente = new Cliente(request.Nome, request.Email);

                _clienteRepository.Adicionar(cliente);

                // Retorna um DTO de resposta
                return new ClienteResponse(cliente.Id, cliente.Nome, cliente.Email);
            }

            public ClienteResponse ObterPorId(Guid id)
            {
                var cliente = _clienteRepository.ObterPorId(id);
                if (cliente == null) return null;

                return new ClienteResponse(cliente.Id, cliente.Nome, cliente.Email);
            }

            public IEnumerable<ClienteResponse> ObterTodos()
            {
                var clientes = _clienteRepository.ObterTodos();
                return clientes.Select(c => new ClienteResponse(c.Id, c.Nome, c.Email));
            }
        }
    }

