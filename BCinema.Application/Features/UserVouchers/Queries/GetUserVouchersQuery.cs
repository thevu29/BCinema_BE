using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Queries;

public class GetUserVouchersQuery : IRequest<IEnumerable<UserVoucherDto>>
{    
    public class GetUserVouchersQueryHandler : IRequestHandler<GetUserVouchersQuery, IEnumerable<UserVoucherDto>>
    {
        private readonly IUserVoucherRepository _userVoucherRepository;
        private readonly IMapper _mapper;

        public GetUserVouchersQueryHandler(IUserVoucherRepository userVoucherRepository, IMapper mapper)
        {
            _userVoucherRepository = userVoucherRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<UserVoucherDto>> Handle(
            GetUserVouchersQuery request,
            CancellationToken cancellationToken)
        {
            var userVouchers = await _userVoucherRepository.GetUserVouchersAsync(cancellationToken);

            return _mapper.Map<IEnumerable<UserVoucherDto>>(userVouchers);
        }
    }
}
