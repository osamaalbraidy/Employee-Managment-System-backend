namespace Application.TimeSheets
{
    public class TimeSheetDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string Employee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string WorkSummary { get; set; }
    }
}