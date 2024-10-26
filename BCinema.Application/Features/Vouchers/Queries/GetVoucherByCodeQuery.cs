using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Vouchers.Queries;

public class GetVoucherByCodeQuery : IRequest<VoucherDto>
{
    public string Code { get; set; } = default!;

    public class GetVoucherByCodeQueryHandler : IRequestHandler<GetVoucherByCodeQuery, VoucherDto>
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public GetVoucherByCodeQueryHandler(IVoucherRepository voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        public async Task<VoucherDto> Handle(GetVoucherByCodeQuery request, CancellationToken cancellationToken)
        {
            var voucher = await _voucherRepository
                .GetByCoedAsync(request.Code, cancellationToken)
                ?? throw new NotFoundException(nameof(Vouchers));

            return _mapper.Map<VoucherDto>(voucher);
        }
    }
}