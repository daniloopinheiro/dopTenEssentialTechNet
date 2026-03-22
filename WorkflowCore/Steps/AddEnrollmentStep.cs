using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace EnrollmentWorkflow.Steps;

public class AddEnrollmentStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("AddEnrollmentStep: registrando matrícula…");
        return ExecutionResult.Next();
    }
}
