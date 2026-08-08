using VibeScan.Adapters.Inbound.Http.Controllers;
using VibeScan.Adapters.Outbound.Cache;
using VibeScan.Adapters.Outbound.Ia;
using VibeScan.Adapters.Outbound.Prompt;
using VibeScan.Application.UseCases;
using VibeScan.Domain.Ports.In;
using VibeScan.Domain.Ports.Out;

namespace VibeScan.API.DependencyInjection;

/// <summary>
/// Extension methods para manter o Program.cs limpo e legível.
/// Cada método agrupa serviços relacionados — SRP aplicado à configuração.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVibeScanDomain(this IServiceCollection services)
    {
        // Use Cases registrados como Scoped (por requisição HTTP)
        services.AddScoped<IAnalisarCodigoUseCase, AnalisarCodigoUseCase>();
        services.AddScoped<IObterHistoricoUseCase, ObterHistoricoUseCase>();
        return services;
    }

    public static IServiceCollection AddVibeScanAdapters(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Cache em memória (port de repositório)
        services.AddMemoryCache();
        services.AddSingleton<IAnaliseRepositoryPort, MemoryCacheRepositoryAdapter>();

        // Prompt builder
        services.AddSingleton<IPromptBuilderPort, PromptBuilderAdapter>();

        // Configurações do Claude carregadas do appsettings
        services.Configure<ClaudeSettings>(
            configuration.GetSection(ClaudeSettings.SectionName));

        // HttpClient para Claude com base URL configurada
        services.AddHttpClient<IAnalisadorPort, ClaudeAnalisadorAdapter>(client =>
        {
            var baseUrl = configuration[$"{ClaudeSettings.SectionName}:BaseUrl"]
                          ?? "https://api.anthropic.com";
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout     = TimeSpan.FromSeconds(60);
        });

        return services;
    }

    public static IServiceCollection AddVibeScanControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(AnaliseController).Assembly);

        return services;
    }
}
