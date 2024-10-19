using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.UserVouchers.Queries;

public class GetByUIdIdQuery : IRequest<IEnumerable<UserVoucherDto>>
{
    public Guid UserId { get; set; }
    
    public class GetByUIdQueryHandler : IRequestHandler<GetByUIdIdQuery, IEnumerable<UserVoucherDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetByUIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<UserVoucherDto>> Handle(GetByUIdIdQuery request, CancellationToken cancellationToken)
        {
            var userVouchers = await _context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<UserVoucherDto>>(userVouchers);
        }
    }
}