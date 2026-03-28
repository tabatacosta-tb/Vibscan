using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendasLux.Application.DTOs
{
    public record ClienteRequest(string Nome, string Email);
    public record ClienteResponse(Guid Id, string Nome, string Email);
}
