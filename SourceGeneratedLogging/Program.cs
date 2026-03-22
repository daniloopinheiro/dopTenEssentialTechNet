using Microsoft.Extensions.Logging;
using SourceGeneratedLogging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddSimpleConsole(options => options.SingleLine = true);
});

var logger = loggerFactory.CreateLogger("Pedidos");
var order = new Order(42);

Log.OrderProcessingStarted(logger, order.Id);
