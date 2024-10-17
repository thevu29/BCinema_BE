using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Vouchers.Queries;

public class GetAllVoucherQuery : IRequest<IEnumerable<VoucherDto>>
{
    public class GetAllVoucherQueryHandler : IRequestHandler<GetAllVoucherQuery, IEnumerable<VoucherDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllVoucherQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VoucherDto>> Handle(GetAllVoucherQuery request, CancellationToken cancellationToken)
        {
            var vouchers = await _context.Vouchers.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<VoucherDto>>(vouchers);
        }
    }
}