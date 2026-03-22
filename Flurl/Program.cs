using System.Net.Http;
using FlurlHttpResilience;
using Microsoft.Extensions.Logging;
using Polly;

using var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Information));
var logger = loggerFactory.CreateLogger("FlurlDemo");

// Fallback com ExecuteAsync<Result<T>> exige Policy<Result<T>>.FallbackAsync; use CreateFallbackExcluding401 só nesse cenário.
var policy = AuthenticatedPolicies.CreateAuthenticatedPolicy();

var context = new Context
{
    [PollyContextExtensions.LoggerKey] = logger,
    [PolicyConstants.AccessTokenKey] = "token-expirado-demo"
};

var result = await policy.ExecuteAsync(
    async (ctx, _) => await SimulatedHttpCallAsync(ctx).ConfigureAwait(false),
    context,
    CancellationToken.None).ConfigureAwait(false);

Console.WriteLine(result.IsSuccess ? $"Sucesso: {result.Value}" : $"Falha: {result.Error}");

static async Task<Result<string>> SimulatedHttpCallAsync(Context ctx)
{
    var token = ctx.ContainsKey(PolicyConstants.AccessTokenKey)
        ? ctx[PolicyConstants.AccessTokenKey]?.ToString()
        : null;

    await Task.Delay(20).ConfigureAwait(false);

    // Primeira tentativa: simula 401 para disparar refresh; após renovação do token, sucesso.
    if (token is null || token.StartsWith("token-expirado", StringComparison.Ordinal))
    {
        // Em produção: Flurl lança FlurlHttpException com StatusCode 401.
        throw new HttpRequestException("401 Unauthorized (demo)");
    }

    return Result<string>.Ok($"Dados com token válido ({token})");
}
