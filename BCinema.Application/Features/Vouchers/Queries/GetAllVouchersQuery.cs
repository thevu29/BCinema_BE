using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Vouchers.Queries;

public class GetAllVouchersQuery : IRequest<IEnumerable<VoucherDto>>
{
    public class GetAllVouchersQueryHandler(
        IVoucherRepository voucherRepository,
        IMapper mapper) : IRequestHandler<GetAllVouchersQuery, IEnumerable<VoucherDto>>
    {
        public async Task<IEnumerable<VoucherDto>> Handle(GetAllVouchersQuery request, CancellationToken cancellationToken)
        {
            var vouchers = await voucherRepository.GetVouchersAsync(cancellationToken);
            return mapper.Map<IEnumerable<VoucherDto>>(vouchers);
        }
    }
}