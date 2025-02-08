using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;


namespace Application.Departments
{
    public class Create
    {
        public class Command : IRequest<Result<DepartmentDto>>
        {
            public Department Department { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Department.Name).NotEmpty().WithMessage("Department name should not be empty");
            }
        }

        public class Handler : IRequestHandler<Command, Result<DepartmentDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<DepartmentDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return Result<DepartmentDto>.Failure("User not found");

                var department = new Department
                {
                    Name = request.Department.Name,
                };

                _context.Departments.Add(department);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<DepartmentDto>.Failure("Failed to create department");

                var DepartmentDto = _mapper.Map<DepartmentDto>(department);

                return Result<DepartmentDto>.Success(DepartmentDto);
            }
        }
    }
}