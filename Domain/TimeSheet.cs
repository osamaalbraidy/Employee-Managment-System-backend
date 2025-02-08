namespace Domain
{
    public class TimeSheet
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string WorkSummary { get; set; }
    }
}