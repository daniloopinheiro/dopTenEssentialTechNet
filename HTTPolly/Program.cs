using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options => options.SingleLine = true);
builder.Logging.AddFilter("System.Net.Http", LogLevel.Warning);

const string ClientName = "jsonplaceholder";

builder.Services.AddHttpClient(ClientName, client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("HTTPolly/1.0");
})
.AddStandardResilienceHandler();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("HTTPolly");
var factory = app.Services.GetRequiredService<IHttpClientFactory>();
var http = factory.CreateClient(ClientName);

logger.LogInformation("Pedido HTTP com pipeline padrão de resiliência (retry, circuit breaker, timeout).");

var response = await http.GetAsync("posts/1", CancellationToken.None);
response.EnsureSuccessStatusCode();

var body = await response.Content.ReadAsStringAsync(CancellationToken.None);
logger.LogInformation("Resposta obtida com sucesso. Comprimento do corpo: {Length} bytes.", body.Length);

Console.WriteLine(body);
