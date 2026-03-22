using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace EnrollmentWorkflow.Steps;

public class ScheduleEvaluationCompensationStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("ScheduleEvaluationCompensationStep: cancelando agendamento…");
        return ExecutionResult.Next();
    }
}
