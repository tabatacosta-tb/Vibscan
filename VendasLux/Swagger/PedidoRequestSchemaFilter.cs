using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VendasLux.Application.DTOs;

namespace VendasLux.API.Swagger
{
    // Esta classe implementa a interface do Swagger para alterar como os exemplos aparecem
    public class PedidoRequestSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // Verifica se o Swagger está gerando a tela do nosso PedidoRequest
            if (context.Type == typeof(PedidoRequest))
            {
                // Sobrescreve o exemplo padrão pelo nosso JSON limpo
                schema.Example = new OpenApiObject
                {
                    ["clienteNome"] = new OpenApiString("Tabata"),
                    ["itens"] = new OpenApiArray
                    {
                        new OpenApiObject
                        {
                            ["produtoNome"] = new OpenApiString("TV"),
                            ["quantidade"] = new OpenApiInteger(2)
                        }
                    }
                };
            }
        }
    }
}