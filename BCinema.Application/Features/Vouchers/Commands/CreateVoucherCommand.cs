using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Vouchers.Commands;

public class CreateVoucherCommand : IRequest<VoucherDto>
{
    public string Code { get; set; }
    public int Discount { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public DateTime ExpireAt { get; set; }
    
    public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, VoucherDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateVoucherCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VoucherDto> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            var voucher = _mapper.Map<Voucher>(request);
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VoucherDto>(voucher);
        }
    }
}