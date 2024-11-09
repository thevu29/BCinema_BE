using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Queries;

public class GetUserVoucherByUIdAndVIdQuery : IRequest<UserVoucherDto?>
{
    public Guid UserId { get; init; }
    public Guid VoucherId { get; init; }
    
    public class GetUserByUIdAndVIdQueryHandler(
        IUserVoucherRepository userVoucherRepository,
        IUserRepository userRepository,
        IVoucherRepository voucherRepository,
        IMapper mapper)
        : IRequestHandler<GetUserVoucherByUIdAndVIdQuery, UserVoucherDto?>
    {
        public async Task<UserVoucherDto?> Handle(GetUserVoucherByUIdAndVIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository
                .GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User));

            var voucher = await voucherRepository
                .GetByIdAsync(request.VoucherId, cancellationToken)
                ?? throw new NotFoundException(nameof(Voucher));

            var userVoucher = await userVoucherRepository
                .GetUserVoucherByUIdAndVIdAsync(user.Id, voucher.Id, cancellationToken);

            return userVoucher != null ? mapper.Map<UserVoucherDto>(userVoucher) : null;
        }
    }
}