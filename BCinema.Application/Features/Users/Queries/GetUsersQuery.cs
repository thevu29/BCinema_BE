using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.ToListAsync(cancellationToken);
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
        }
    }
}
