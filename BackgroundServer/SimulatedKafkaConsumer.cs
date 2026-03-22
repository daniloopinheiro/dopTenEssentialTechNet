namespace BackgroundServer;

/// <summary>
/// Simula consumo de mensagens (sem broker real) para demonstrar o fluxo do worker.
/// </summary>
internal sealed class SimulatedKafkaConsumer
{
    private int _delivered;

    /// <summary>Após algumas mensagens, retorna null para encerrar a demonstração.</summary>
    public ConsumerResult? Consume(CancellationToken cancellationToken)
    {
        Task.Delay(TimeSpan.FromSeconds(1.5), cancellationToken).GetAwaiter().GetResult();

        if (_delivered >= 3)
            return null;

        _delivered++;
        var json = $$"""{"enrollmentId":{{_delivered}}}""";
        return new ConsumerResult { Message = new ConsumerMessage { Value = json } };
    }

    public void StoreOffset(ConsumerResult result)
    {
        // Em Kafka real: commit de offset por partição.
    }

    public void Commit()
    {
        // Em Kafka real: commit síncrono do batch.
    }
}
