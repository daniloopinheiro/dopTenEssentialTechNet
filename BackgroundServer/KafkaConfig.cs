namespace BackgroundServer;

public sealed class KafkaConfig
{
    public const string Section = "Kafka";

    public string BootstrapServers { get; init; } = "localhost:9092";

    public string Topic { get; init; } = "enrollment-created";

    public string GroupId { get; init; } = "charging-workers";
}
