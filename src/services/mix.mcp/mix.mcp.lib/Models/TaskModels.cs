namespace Mix.MCP.Lib.Models
{
    /// <summary>
    /// Represents the current state of a task
    /// </summary>
    public class TaskState
    {
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
        public string CurrentTask { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }
    }

    /// <summary>
    /// Represents the status of a task
    /// </summary>
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Pending,
        Failed,
        Completed,
        Cancelled
    }

    /// <summary>
    /// Represents an entry in the task history
    /// </summary>
    public class TaskHistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string Response { get; set; }
    }
}