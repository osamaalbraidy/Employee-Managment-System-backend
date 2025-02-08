using Application.Core;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Departments
{
    public class Details
    {
        public class Query : IRequest<Result<DepartmentDto>>
        {
            public Guid DepartmentId { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<DepartmentDto>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            private readonly DataContext _context;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            public async Task<Result<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return Result<DepartmentDto>.Failure("User not found");
                var department = await _context.Departments
                    .FirstOrDefaultAsync(c => c.Id == request.DepartmentId);
                if (department == null) return Result<DepartmentDto>.Failure("Department not found");
                var newDepartment = _mapper.Map<DepartmentDto>(department);

                return Result<DepartmentDto>.Success(newDepartment);
            }
        }
    }
}