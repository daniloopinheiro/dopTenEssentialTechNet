using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace EnrollmentWorkflow.Steps;

public class ProcessPaymentCompensationStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("ProcessPaymentCompensationStep: estornando pagamento…");
        return ExecutionResult.Next();
    }
}
