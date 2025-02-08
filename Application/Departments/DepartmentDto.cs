using Application.Employees;

namespace Application.Departments
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        // public ICollection<EmployeeDto> Employees { get; set; }
    }
}