using Application.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Departments
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public DepartmentDto Department { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Department.Name).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var department = await _context.Departments.FindAsync(request.Id);

                if (department == null) return Result<Unit>.Failure("Department not found");

                request.Department.Id = request.Id;
                _mapper.Map(request.Department, department);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update the department");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}