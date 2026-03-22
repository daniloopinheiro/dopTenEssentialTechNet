using EnrollmentWorkflow.Steps;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace EnrollmentWorkflow;

public class MainWorkflow : IWorkflow<NewEnrollmentFlowData>
{
    public string Id => "new-enrollment-workflow";

    public int Version => 1;

    public void Build(IWorkflowBuilder<NewEnrollmentFlowData> builder)
    {
        builder
            .Saga(saga => saga
                .StartWith<AddEnrollmentStep>()
                    .CompensateWith<AddEnrollmentCompensationStep>()
                .Then<ProcessPaymentStep>()
                    .CompensateWith<ProcessPaymentCompensationStep>()
                .Then<ScheduleEvaluationStep>()
                    .CompensateWith<ScheduleEvaluationCompensationStep>()
            )
            .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(30));
    }
}
