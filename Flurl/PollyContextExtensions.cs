using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;

namespace FlurlHttpResilience;

public static class PollyContextExtensions
{
    public const string LoggerKey = "Logger";

    public static ILogger GetLoggerService(this Context context)
    {
        if (context.TryGetValue(LoggerKey, out var value) && value is ILogger logger)
            return logger;

        return NullLogger.Instance;
    }

    /// <summary>Remove o token expirado do contexto para forçar renovação na próxima tentativa.</summary>
    public static void RemoveToken(this Context context, string key)
    {
        context.Remove(key);
    }

    public static async Task<string> GetOrRefreshToken(this Context context)
    {
        await Task.Delay(10).ConfigureAwait(false);
        // Em produção: chamar IdentityServer, Azure AD, etc.
        return "access-token-" + Guid.NewGuid().ToString("N")[..12];
    }
}
