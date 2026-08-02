using VibeScan.Domain.Ports.Out;

namespace VibeScan.Adapters.Outbound.Prompt;

/// <summary>
/// Adapter responsável exclusivamente por construir o prompt enviado à IA.
/// SRP: essa classe só faz uma coisa — montar texto estruturado para análise.
/// </summary>
public sealed class PromptBuilderAdapter : IPromptBuilderPort
{
    public string Construir(string codigo, string? promptOriginal)
    {
        var secaoPrompt = string.IsNullOrWhiteSpace(promptOriginal)
            ? "Nenhum prompt original fornecido pelo usuário."
            : $"PROMPT ORIGINAL DO USUÁRIO:\n{promptOriginal}";

        return @$"Você é um arquiteto de software sênior especializado em C# e .NET.
Analise o código abaixo com foco em arquitetura, segurança, boas práticas e performance.

Retorne APENAS um JSON válido, sem texto adicional, sem markdown, sem explicações fora do JSON.
Use exatamente este schema:

{{
  ""resumo"": ""descrição objetiva do que o código faz em até 4 linhas"",
  ""score"": <inteiro de 0 a 10>,
  ""arquiteturaRecomendada"": ""nome do padrão recomendado com justificativa curta"",
  ""promptMelhorado"": ""prompt completo e melhorado para o usuário regenerar o código corretamente"",
  ""problemas"": [
    {{
      ""categoria"": ""<Segurança | Arquitetura | Boas Práticas | Performance>"",
      ""descricao"": ""descrição clara do problema encontrado"",
      ""sugestao"": ""como corrigir este problema"",
      ""severidade"": ""<Critico | Aviso | Info>""
    }}
  ]
}}

Regras de score:
- 9-10: código production-ready com boas práticas e arquitetura sólida
- 7-8: bom código com pequenas melhorias necessárias
- 5-6: código funcional mas com problemas relevantes
- 3-4: código com problemas sérios de arquitetura ou segurança
- 0-2: código crítico, inseguro ou sem estrutura alguma

{secaoPrompt}

CÓDIGO PARA ANALISAR:
```csharp
{codigo}
```";
    }
}
