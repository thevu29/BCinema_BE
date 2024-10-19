using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.Vouchers.Commands;

public class UpdateVoucherCommand : IRequest<VoucherDto>
{
    public Guid Id { get; set; }
    public int Discount { get; set; }
    public string? Description { get; set; }
    public DateTime ExpireAt { get; set; }
        
    public class UpdateVoucherCommandHandler : IRequestHandler<UpdateVoucherCommand, VoucherDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateVoucherCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VoucherDto> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
        {
            var voucher = await _context.Vouchers.FindAsync(request.Id) 
                            ?? throw new NotFoundException(nameof(Voucher), request.Id);

            _mapper.Map(request, voucher);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<VoucherDto>(voucher);
        }
    }
}