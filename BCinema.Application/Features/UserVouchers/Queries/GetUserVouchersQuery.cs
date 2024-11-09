using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Queries;

public class GetUserVouchersQuery : IRequest<IEnumerable<UserVoucherDto>>
{    
    public class GetUserVouchersQueryHandler(IUserVoucherRepository userVoucherRepository, IMapper mapper)
        : IRequestHandler<GetUserVouchersQuery, IEnumerable<UserVoucherDto>>
    {
        public async Task<IEnumerable<UserVoucherDto>> Handle(GetUserVouchersQuery request, CancellationToken cancellationToken)
        {
            var userVouchers = await userVoucherRepository.GetUserVouchersAsync(cancellationToken);

            return mapper.Map<IEnumerable<UserVoucherDto>>(userVouchers);
        }
    }
}
