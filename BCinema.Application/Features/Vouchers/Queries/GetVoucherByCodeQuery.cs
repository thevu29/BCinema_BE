using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Vouchers.Queries;

public class GetVoucherByCodeQuery : IRequest<VoucherDto>
{
    public string Code { get; set; } = default!;
    public class GetVoucherByCodeQueryHandle : IRequestHandler<GetVoucherByCodeQuery, VoucherDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetVoucherByCodeQueryHandle(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VoucherDto> Handle(GetVoucherByCodeQuery request, CancellationToken cancellationToken)
        {
            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(v => v.Code == request.Code, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Vouchers), request.Code);
            return _mapper.Map<VoucherDto>(voucher);
        }
    }
}