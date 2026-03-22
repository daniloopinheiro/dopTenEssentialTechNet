using Microsoft.Extensions.Logging;

namespace SourceGeneratedLogging;

public partial class Log
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Iniciando processamento do pedido {OrderId}")]
    public static partial void OrderProcessingStarted(ILogger logger, int orderId);
}
