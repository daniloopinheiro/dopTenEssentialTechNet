using Microsoft.Extensions.Logging;

namespace BackgroundServer;

/// <summary>Handler registrado como Scoped — resolvido dentro de um escopo por mensagem.</summary>
public sealed class ProcessChargingHandler(ILogger<ProcessChargingHandler> logger)
{
    public Task HandleAsync(EnrollmentCreatedEvent? message, CancellationToken cancellationToken)
    {
        if (message is null)
            return Task.CompletedTask;

        logger.LogInformation(
            "Processando cobrança para matrícula {EnrollmentId}",
            message.EnrollmentId);

        return Task.CompletedTask;
    }
}
