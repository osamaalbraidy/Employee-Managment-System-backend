using Application.Departments;
using Application.Employees;
using Application.TimeSheets;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Employee, Employee>();
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.Department, o => o.MapFrom(s => s.Department.Name))
                .ReverseMap();

            CreateMap<TimeSheet, TimeSheet>();
            CreateMap<TimeSheet, TimeSheetDto>()
                .ForMember(d => d.Employee, o => o.MapFrom(s => s.Employee.FirstName + " " + s.Employee.LastName))
                .ReverseMap();

            CreateMap<Department, Department>();
            CreateMap<Department, DepartmentDto>().ReverseMap();
        }
    }
}