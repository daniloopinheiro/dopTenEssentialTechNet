namespace BackgroundServer;

/// <summary>Equivalente conceitual ao erro de consumo do cliente Kafka (ex.: Confluent.Kafka.ConsumeException).</summary>
public sealed class ConsumeException : Exception
{
    public ConsumeException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
