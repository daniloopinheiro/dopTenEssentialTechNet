using EnrollmentWorkflow;
using EnrollmentWorkflow.Steps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;

var services = new ServiceCollection();
services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
services.AddWorkflow();

services.AddTransient<AddEnrollmentStep>();
services.AddTransient<AddEnrollmentCompensationStep>();
services.AddTransient<ProcessPaymentStep>();
services.AddTransient<ProcessPaymentCompensationStep>();
services.AddTransient<ScheduleEvaluationStep>();
services.AddTransient<ScheduleEvaluationCompensationStep>();

await using var provider = services.BuildServiceProvider();
var host = provider.GetRequiredService<IWorkflowHost>();

host.RegisterWorkflow<MainWorkflow, NewEnrollmentFlowData>();

host.Start();

Console.WriteLine("Iniciando workflow new-enrollment-workflow…");
var instanceId = await host.StartWorkflow("new-enrollment-workflow", 1, new NewEnrollmentFlowData());
Console.WriteLine($"Instância: {instanceId}");

await Task.Delay(TimeSpan.FromSeconds(2));

host.Stop();
