using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Application.DTOs;

namespace VendasLux.Application.Interfaces
{
    public interface IClienteService
    {
        IEnumerable<ClienteResponse> ObterTodos();
        ClienteResponse ObterPorId(Guid id);
        ClienteResponse Adicionar(ClienteRequest request);
    }
}
