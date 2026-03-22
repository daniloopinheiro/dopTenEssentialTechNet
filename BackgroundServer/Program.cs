using BackgroundServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection(KafkaConfig.Section));
builder.Services.AddScoped<ProcessChargingHandler>();
builder.Services.AddHostedService<EnrollmentCreatedEventConsumerHostedService>();

builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Error);

using var host = builder.Build();
await host.RunAsync();
