using AutoMapper;
using BCinema.Application.DTOs;
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
        private readonly IMapper _mapper;

        public GetUserByUIdAndVIdQueryHandler(IUserVoucherRepository userVoucherRepository, IMapper mapper)
        {
            _userVoucherRepository = userVoucherRepository;
            _mapper = mapper;
        }
        
        public async Task<UserVoucherDto> Handle(
            GetUserVoucherByUIdAndVIdQuery request,
            CancellationToken cancellationToken)
        {
            var userVoucher = await _userVoucherRepository
                .GetUserVoucherByUIdAndVIdAsync(request.UserId, request.VoucherId, cancellationToken);

            return _mapper.Map<UserVoucherDto>(userVoucher);
        }
    }
}