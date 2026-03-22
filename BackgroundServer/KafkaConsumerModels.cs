namespace BackgroundServer;

/// <summary>Forma mínima compatível com o padrão result.Message.Value do cliente Kafka.</summary>
internal sealed class ConsumerResult
{
    public ConsumerMessage? Message { get; init; }
}

internal sealed class ConsumerMessage
{
    public string? Value { get; init; }
}
