namespace Mix.Communicator.Models
{
    public sealed class EmailMessageModel
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string To { get; set; }
        public string? CC { get; set; }
        public string? From { get; set; }
        public string? FromName { get; set; }
    }
}
