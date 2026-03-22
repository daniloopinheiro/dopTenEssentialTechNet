using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackgroundServer;

/// <summary>
/// Worker em segundo plano no mesmo estilo da apresentação: BackgroundService + escopo por mensagem.
/// </summary>
public sealed class EnrollmentCreatedEventConsumerHostedService(
    IOptions<KafkaConfig> kafkaOptions,
    IServiceScopeFactory scopeFactory,
    ILogger<EnrollmentCreatedEventConsumerHostedService> logger,
    IHostApplicationLifetime lifetime)
    : BackgroundService
{
    private readonly SimulatedKafkaConsumer _consumer = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Consumer iniciado (tópico simulado: {Topic}, grupo: {GroupId})",
            kafkaOptions.Value.Topic,
            kafkaOptions.Value.GroupId);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    if (result is null)
                    {
                        logger.LogInformation("Simulação concluída; encerrando host.");
                        lifetime.StopApplication();
                        break;
                    }

                    if (result?.Message?.Value == null)
                        continue;

                    var message = JsonSerializer.Deserialize<EnrollmentCreatedEvent>(
                        result.Message.Value,
                        JsonDefaults.Options);

                    await using (var scope = scopeFactory.CreateAsyncScope())
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<ProcessChargingHandler>();
                        await handler.HandleAsync(message, stoppingToken).ConfigureAwait(false);
                    }

                    _consumer.StoreOffset(result);
                    _consumer.Commit();
                }
                catch (ConsumeException ex)
                {
                    logger.LogError(ex, "Falha ao consumir mensagem do Kafka (simulado).");
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Cancelamento solicitado.");
        }
    }
}
