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
    
    public class CreateVoucherCommandHandler(IVoucherRepository voucherRepository, IMapper mapper)
        : IRequestHandler<CreateVoucherCommand, VoucherDto>
    {
        public async Task<VoucherDto> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            request.ExpireAt = DateTime.SpecifyKind(request.ExpireAt, DateTimeKind.Utc);

            var voucher = mapper.Map<Voucher>(request);

            await voucherRepository.AddVoucherAsync(voucher, cancellationToken);
            await voucherRepository.SaveChangesAsync(cancellationToken);

            return mapper.Map<VoucherDto>(voucher);
        }
    }
}