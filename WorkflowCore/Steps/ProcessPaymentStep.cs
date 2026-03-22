using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace EnrollmentWorkflow.Steps;

public class ProcessPaymentStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("ProcessPaymentStep: processando pagamento…");
        return ExecutionResult.Next();
    }
}
