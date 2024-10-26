using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Vouchers.Commands;

public class CreateVoucherCommand : IRequest<VoucherDto>
{
    public string Code { get; set; } = default!;
    public int Discount { get; set; }
    public string? Description { get; set; }
    public DateTime ExpireAt { get; set; }
    
    public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, VoucherDto>
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public CreateVoucherCommandHandler(IVoucherRepository voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        public async Task<VoucherDto> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            var voucher = _mapper.Map<Voucher>(request);

            await _voucherRepository.AddVoucherAsync(voucher, cancellationToken);
            await _voucherRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VoucherDto>(voucher);
        }
    }
}