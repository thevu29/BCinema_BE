using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Queries;

public class GetUserVoucherByUIdAndVIdQuery : IRequest<UserVoucherDto>
{
    public Guid UserId { get; set; }
    public Guid VoucherId { get; set; }
    
    public class GetUserByUIdAndVIdQueryHandler : IRequestHandler<GetUserVoucherByUIdAndVIdQuery, UserVoucherDto>
    {
        private readonly IUserVoucherRepository _userVoucherRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public GetUserByUIdAndVIdQueryHandler(
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
            GetUserVoucherByUIdAndVIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository
                .GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User));

            var voucher = await _voucherRepository
                .GetByIdAsync(request.VoucherId, cancellationToken)
                ?? throw new NotFoundException(nameof(Voucher));

            var userVoucher = await _userVoucherRepository
                .GetUserVoucherByUIdAndVIdAsync(request.UserId, request.VoucherId, cancellationToken);

            return _mapper.Map<UserVoucherDto>(userVoucher);
        }
    }
}