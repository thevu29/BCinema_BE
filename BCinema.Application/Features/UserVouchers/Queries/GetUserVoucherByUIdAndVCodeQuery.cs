using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Queries;

public class GetUserVoucherByUIdAndVCodeQuery : IRequest<UseVoucherDto>
{
    public Guid UserId { get; set; }
    public string Code { get; set; } = default!;

    public class GetUserVoucherByUIdAndVCodeQueryHandler(
        IUserVoucherRepository userVoucherRepository,
        IUserRepository userRepository,
        IVoucherRepository voucherRepository,
        IMapper mapper) : IRequestHandler<GetUserVoucherByUIdAndVCodeQuery, UseVoucherDto>
    {
        public async Task<UseVoucherDto> Handle(GetUserVoucherByUIdAndVCodeQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken) 
                       ?? throw new NotFoundException(nameof(User));

            var voucher = await voucherRepository.GetByCoedAsync(request.Code, cancellationToken)
                          ?? throw new NotFoundException(nameof(Voucher));

            var userVoucher = await userVoucherRepository
                .GetUserVoucherByUIdAndVIdAsync(user.Id, voucher.Id, cancellationToken);

            var useVoucherDto = new UseVoucherDto { VoucherId = voucher.Id, UserId = user.Id, IsUsed = false };
            
            if (userVoucher is not null)
            {
                useVoucherDto.IsUsed = true;
            }
            
            return useVoucherDto;
        }
    }
}