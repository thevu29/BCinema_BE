using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Queries;

public class GetUserVoucherByUIdAndVCodeQuery : IRequest<UserVoucherDto?>
{
    public Guid UserId { get; set; }
    public string Code { get; set; } = default!;

    public class GetUserVoucherByUIdAndVCodeQueryHandler(
        IUserVoucherRepository userVoucherRepository,
        IUserRepository userRepository,
        IVoucherRepository voucherRepository,
        IMapper mapper) : IRequestHandler<GetUserVoucherByUIdAndVCodeQuery, UserVoucherDto?>
    {
        public async Task<UserVoucherDto?> Handle(GetUserVoucherByUIdAndVCodeQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken) 
                       ?? throw new NotFoundException(nameof(User));

            var voucher = await voucherRepository.GetByCoedAsync(request.Code, cancellationToken)
                          ?? throw new NotFoundException(nameof(Voucher));

            var userVoucher = await userVoucherRepository
                .GetUserVoucherByUIdAndVIdAsync(user.Id, voucher.Id, cancellationToken);

            return userVoucher != null ? mapper.Map<UserVoucherDto>(userVoucher) : null; 
        }
    }
}