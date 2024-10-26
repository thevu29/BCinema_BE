using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Commands;

public class CreateUserVoucherCommand : IRequest<UserVoucherDto>
{
    public Guid UserId { get; set; }
    public Guid? VoucherId { get; set; }
    public string Code { get; set; } = default!;

    public class CreateUserVoucherCommandHandler : IRequestHandler<CreateUserVoucherCommand, UserVoucherDto>
    {
        private readonly IUserVoucherRepository _userVoucherRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public CreateUserVoucherCommandHandler(
            IUserVoucherRepository userVoucherRepository,
            IUserRepository userRepository,
            IVoucherRepository voucherRepository,
            IMapper mapper)
        {
            _userVoucherRepository = userVoucherRepository;
            _userRepository = userRepository;
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }
        
        public async Task<UserVoucherDto> Handle(
            CreateUserVoucherCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User));

            var voucher = await _voucherRepository.GetByCoedAsync(request.Code, cancellationToken)
                ?? throw new BadRequestException("Invalid voucher code");

            var usingVoucher = await _userVoucherRepository
                .GetUserVoucherByUIdAndVIdAsync(request.UserId, voucher.Id, cancellationToken);

            if (usingVoucher != null) throw new BadRequestException("User already used this voucher");

            if (voucher.ExpireAt < DateTime.UtcNow) throw new BadRequestException("Voucher is expired");

            request.VoucherId = voucher.Id;
            var userVoucher = _mapper.Map<UserVoucher>(request);

            await _userVoucherRepository.AddUserVoucherAsync(userVoucher, cancellationToken);
            await _userVoucherRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserVoucherDto>(userVoucher);
        }
    }
}
