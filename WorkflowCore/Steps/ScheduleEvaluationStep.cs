using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace EnrollmentWorkflow.Steps;

public class ScheduleEvaluationStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("ScheduleEvaluationStep: agendando avaliação…");
        return ExecutionResult.Next();
    }
}
