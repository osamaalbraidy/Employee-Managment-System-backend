using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class Seed
    {
        public static async System.Threading.Tasks.Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            await SeedUsers(userManager);
            await SeedDepartments(context);
            await SeedEmployees(context);
            await SeedTimeSheets(context);
        }

        private static async System.Threading.Tasks.Task SeedUsers(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser { FirstName = "John", LastName = "Doe", UserName = "john.doe", Email = "john@example.com" },
                    new AppUser { FirstName = "Jane", LastName = "Smith", UserName = "jane.smith", Email = "jane@example.com" }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }

        private static async System.Threading.Tasks.Task SeedDepartments(DataContext context)
        {
            if (!context.Departments.Any())
            {
                var departments = new List<Department>
                {
                    new Department { Id = Guid.NewGuid(), Name = "Human Resources" },
                    new Department { Id = Guid.NewGuid(), Name = "IT" },
                    new Department { Id = Guid.NewGuid(), Name = "Finance" }
                };

                await context.Departments.AddRangeAsync(departments);
                await context.SaveChangesAsync();
            }
        }

        private static async System.Threading.Tasks.Task SeedEmployees(DataContext context)
        {
            if (!context.Employees.Any())
            {
                var departments = context.Departments.ToList();
                var employees = new List<Employee>
                {
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Alice",
                        LastName = "Johnson",
                        Email = "alice.johnson@example.com",
                        PhoneNumber = "123-456-7890",
                        DateOfBirth = new DateTime(1990, 5, 10),
                        JobTitle = "HR Manager",
                        DepartmentId = departments.First(d => d.Name == "Human Resources").Id,
                        Salary = 60000,
                        StartDate = DateTime.UtcNow,
                        PhotoPath = "alice.jpg",
                        DocumentPath = "alice_docs.pdf"
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Bob",
                        LastName = "Williams",
                        Email = "bob.williams@example.com",
                        PhoneNumber = "987-654-3210",
                        DateOfBirth = new DateTime(1985, 8, 20),
                        JobTitle = "Software Engineer",
                        DepartmentId = departments.First(d => d.Name == "IT").Id,
                        Salary = 80000,
                        StartDate = DateTime.UtcNow,
                        PhotoPath = "bob.jpg",
                        DocumentPath = "bob_docs.pdf"
                    }
                };

                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();
            }
        }

        private static async System.Threading.Tasks.Task SeedTimeSheets(DataContext context)
        {
            if (!context.TimeSheets.Any())
            {
                var employees = context.Employees.ToList();
                var timeSheets = new List<TimeSheet>
                {
                    new TimeSheet
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = employees.First(e => e.FirstName == "Alice").Id,
                        StartTime = DateTime.UtcNow.AddHours(-8),
                        EndTime = DateTime.UtcNow,
                        WorkSummary = "Reviewed company policies and conducted interviews."
                    },
                    new TimeSheet
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = employees.First(e => e.FirstName == "Bob").Id,
                        StartTime = DateTime.UtcNow.AddHours(-9),
                        EndTime = DateTime.UtcNow,
                        WorkSummary = "Developed new API endpoints and fixed bugs."
                    }
                };

                await context.TimeSheets.AddRangeAsync(timeSheets);
                await context.SaveChangesAsync();
            }
        }
    }
}