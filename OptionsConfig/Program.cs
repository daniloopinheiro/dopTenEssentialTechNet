using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OptionsConfig;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Error);

builder.Services.AddOptions<MongoOptions>()
    .BindConfiguration(MongoOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<MyService>();

using var host = builder.Build();

await host.StartAsync();
try
{
    var service = host.Services.GetRequiredService<MyService>();
    service.ShowConfiguration();
}
finally
{
    await host.StopAsync();
}
