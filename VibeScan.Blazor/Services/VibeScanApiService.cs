using System.Net.Http.Json;
using VibeScan.Blazor.Models;

namespace VibeScan.Blazor.Services;

/// <summary>
/// Serviço que abstrai as chamadas HTTP para a VibeScan API.
/// O Blazor nunca faz fetch direto — passa sempre por aqui (SRP).
/// </summary>
public sealed class VibeScanApiService
{
    private readonly HttpClient _http;

    public VibeScanApiService(HttpClient http)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
    }

    public async Task<AnalisarResponse?> AnalisarAsync(
        string codigo,
        string? promptOriginal,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException("Informe o código para análise.", nameof(codigo));

        var request = new AnalisarRequest(codigo, promptOriginal);

        using var response = await _http.PostAsJsonAsync("/api/analise", request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"Erro ao analisar código. Status: {(int)response.StatusCode} {response.ReasonPhrase}. Resposta: {body}");
        }

        return await response.Content.ReadFromJsonAsync<AnalisarResponse>(cancellationToken: ct);
    }

    public async Task<List<HistoricoItem>> ObterHistoricoAsync(CancellationToken ct = default)
    {
        var items = await _http.GetFromJsonAsync<List<HistoricoItem>>("api/historico", ct);
        return items ?? new List<HistoricoItem>();
    }

    public async Task<AnalisarResponse?> ObterDetalheAsync(Guid id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<AnalisarResponse>($"api/historico/{id}", ct);
}
