using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Vouchers.Queries;
public class GetVoucherByIdQuery : IRequest<VoucherDto>
{
    public Guid Id { get; set; }

    public class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, VoucherDto>
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public GetVoucherByIdQueryHandler(IVoucherRepository voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        public async Task<VoucherDto> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
        {
            var voucher = await _voucherRepository
                .GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Vouchers));

            return _mapper.Map<VoucherDto>(voucher);
        }
    }
}
