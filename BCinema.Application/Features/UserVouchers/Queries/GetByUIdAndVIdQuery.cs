using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.UserVouchers.Queries;

public class GetByUIdAndVIdQuery : IRequest<UserVoucherDto>
{
    public Guid UserId { get; set; }
    public Guid VoucherId { get; set; }
    
    public class GetByUIdAndVIdQueryHandler : IRequestHandler<GetByUIdAndVIdQuery, UserVoucherDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetByUIdAndVIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<UserVoucherDto> Handle(GetByUIdAndVIdQuery request, CancellationToken cancellationToken)
        {
            var userVoucher = await _context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.VoucherId == request.VoucherId,
                    cancellationToken: cancellationToken);

            return _mapper.Map<UserVoucherDto>(userVoucher);
        }
    }
}