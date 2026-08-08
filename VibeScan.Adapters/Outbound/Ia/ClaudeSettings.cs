namespace VibeScan.Adapters.Outbound.Ia;

/// <summary>
/// Configurações fortemente tipadas para a API Claude.
/// Evita magic strings espalhadas pelo código.
/// </summary>
public sealed class ClaudeSettings
{
    public const string SectionName = "Claude";

    public string ApiKey  { get; init; } = string.Empty;
    public string Model   { get; init; } = "claude-sonnet-4-6";
    public int MaxTokens  { get; init; } = 2000;
    public string BaseUrl { get; init; } = "https://api.anthropic.com";
}
