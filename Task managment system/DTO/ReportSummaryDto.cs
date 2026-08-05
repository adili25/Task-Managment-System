namespace Task_managment_system.DTO
{
    public record TaskSummaryReportDto(
    Dictionary<string, int> TasksByStatus,
    int OverdueTasksCount,
    Dictionary<string, int> TasksPerUser,
    int HighPriorityIncompleteCount,
    double CompletionPercentage
        );
}
