using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace EnrollmentWorkflow.Steps;

public class AddEnrollmentCompensationStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("AddEnrollmentCompensationStep: compensando matrícula…");
        return ExecutionResult.Next();
    }
}
