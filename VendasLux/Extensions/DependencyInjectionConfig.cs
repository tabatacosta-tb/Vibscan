using VendasLux.Application.Interfaces;
using VendasLux.Application.Services;
using VendasLux.Domain.Interfaces;
using VendasLux.Infrastructure.Repositories;

namespace VendasLux.API.Extensions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {           

            // Repositórios (Infraestrutura)
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            // Serviços (Aplicação)
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IProdutoService, ProdutoService>(); 

            return services;
        }
    }
}