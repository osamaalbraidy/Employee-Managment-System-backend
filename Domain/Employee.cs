namespace Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PhotoPath { get; set; }
        public string DocumentPath { get; set; }
        public ICollection<TimeSheet> TimeSheets { get; set; }
    }
}