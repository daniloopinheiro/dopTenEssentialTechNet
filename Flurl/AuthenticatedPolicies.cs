using System.Net;
using System.Net.Http;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Polly;

namespace FlurlHttpResilience;

/// <summary>
/// Cadeia Polly + Flurl no estilo da apresentação: circuit breaker → refresh em 401 → retentativas → fallback.
/// Usa <see cref="Policy"/> (não genérico), compatível com <c>ExecuteAsync&lt;TResult&gt;</c> do Polly 7.
/// </summary>
public static class AuthenticatedPolicies
{
    /// <summary>Compõe as políticas; execute com <c>ExecuteAsync&lt;Result&lt;T&gt;&gt;(...)</c>.</summary>
    public static IAsyncPolicy CreateAuthenticatedPolicy() =>
        CreateCircuitBreaker()
            .WrapAsync(CreateUnauthorizedRefreshPolicy())
            .WrapAsync(CreateTransientRetryPolicy(3));

    private static bool IsUnauthorized(FlurlHttpException exception) =>
        GetStatusCode(exception) == (int)HttpStatusCode.Unauthorized
        || exception.Message.Contains("401", StringComparison.Ordinal);

    private static int? GetStatusCode(FlurlHttpException? exception) =>
        exception is null ? null : (int?)exception.StatusCode;

    private static IAsyncPolicy CreateUnauthorizedRefreshPolicy() =>
        Policy
            .Handle<FlurlHttpException>(IsUnauthorized)
            .Or<HttpRequestException>(ex => ex.Message.Contains("401", StringComparison.Ordinal))
            .RetryAsync(
                retryCount: 1,
                onRetryAsync: async (exception, retryCount, context) =>
                {
                    var logger = context.GetLoggerService();
                    logger.LogInformation("401 Unauthorized; renovando token (tentativa {Retry}).", retryCount);

                    // Remover token expirado do contexto para forçar renovação
                    context.RemoveToken(PolicyConstants.AccessTokenKey);

                    var newToken = await context.GetOrRefreshToken().ConfigureAwait(false);
                    context[PolicyConstants.AccessTokenKey] = newToken;

                    logger.LogInformation("Token renovado.");
                });

    private static IAsyncPolicy CreateCircuitBreaker() =>
        Policy
            .Handle<Exception>(ex => ex is not OperationCanceledException)
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

    private static IAsyncPolicy CreateTransientRetryPolicy(int retries) =>
        Policy
            .Handle<FlurlHttpException>(ex => GetStatusCode(ex) != (int)HttpStatusCode.Unauthorized)
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(
                retries,
                retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)));

    /// <summary>
    /// Fallback genérico que ignora 401 — pode ser combinado com <c>WrapAsync</c> em cadeias <see cref="IAsyncPolicy{TResult}"/>.
    /// </summary>
    public static IAsyncPolicy<Result<T>> CreateFallbackPolicyExcluding401<T>() =>
        Policy<Result<T>>
            .Handle<Exception>(ex => GetStatusCode(ex as FlurlHttpException) != (int)HttpStatusCode.Unauthorized)
            .FallbackAsync(
                Result<T>.Fail("Fallback: operação não concluída."),
                onFallbackAsync: _ => Task.CompletedTask);
}
