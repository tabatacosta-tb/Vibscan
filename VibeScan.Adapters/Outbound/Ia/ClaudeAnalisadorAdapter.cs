using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using VibeScan.Domain.Ports.Out;

namespace VibeScan.Adapters.Outbound.Ia;

/// <summary>
/// Adapter concreto para a API Claude da Anthropic.
/// Implementa IAnalisadorPort — o domínio nunca vê esta classe diretamente.
/// Para trocar por OpenAI: criar OpenAiAnalisadorAdapter e registrar no DI.
/// </summary>
public sealed class ClaudeAnalisadorAdapter : IAnalisadorPort
{
    private readonly HttpClient          _httpClient;
    private readonly ClaudeSettings      _settings;
    private readonly IPromptBuilderPort  _promptBuilder;

    public ClaudeAnalisadorAdapter(
        HttpClient httpClient,
        IOptions<ClaudeSettings> settings,
        IPromptBuilderPort promptBuilder)
    {
        _httpClient    = httpClient    ?? throw new ArgumentNullException(nameof(httpClient));
        _settings      = settings.Value;
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
    }

    public async Task<ResultadoIa> AnalisarAsync(
        string codigo,
        string? promptOriginal,
        CancellationToken ct = default)
    {
        var prompt = _promptBuilder.Construir(codigo, promptOriginal);

        var requestBody = new
        {
            model      = _settings.Model,
            max_tokens = _settings.MaxTokens,
            messages   = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var json    = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/v1/messages")
        {
            Content = content
        };

        request.Headers.Add("x-api-key",         _settings.ApiKey);
        request.Headers.Add("anthropic-version",  "2023-06-01");

        var response = await _httpClient.SendAsync(request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var erro = await response.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"Erro na API Claude [{response.StatusCode}]: {erro}");
        }

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var claudeResp   = JsonSerializer.Deserialize<ClaudeApiResponse>(responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var textoResposta = claudeResp?.Content?.FirstOrDefault()?.Text
            ?? throw new InvalidOperationException("Resposta da Claude veio vazia.");

        return ClaudeResponseParser.Parse(textoResposta);
    }

    // Tipos para deserializar a envelope da API Claude
    private sealed class ClaudeApiResponse
    {
        public List<ClaudeContentBlock>? Content { get; init; }
    }

    private sealed class ClaudeContentBlock
    {
        public string? Text { get; init; }
    }
}
