using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Departments
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<DepartmentDto>>>
        {
            public PagingParams Params { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<PagedList<DepartmentDto>>>
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
            public async Task<Result<PagedList<DepartmentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return Result<PagedList<DepartmentDto>>.Failure("User not found");

                var query = _context.Departments
                    .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                var pagedDepartments = await PagedList<DepartmentDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);

                return Result<PagedList<DepartmentDto>>.Success(pagedDepartments);
            }
        }
    }
}