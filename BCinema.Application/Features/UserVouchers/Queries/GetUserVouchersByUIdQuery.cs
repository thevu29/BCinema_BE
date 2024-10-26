using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Queries
{
    public class GetUserVouchersByUIdQuery : IRequest<IEnumerable<UserVoucherDto>>
    {
        public Guid UserId { get; set; }

        public class GetUserVouchersByUIdQueryHandler 
            : IRequestHandler<GetUserVouchersByUIdQuery, IEnumerable<UserVoucherDto>>
        {
            private readonly IUserVoucherRepository _userVoucherRepository;
            private readonly IMapper _mapper;

            public GetUserVouchersByUIdQueryHandler(IUserVoucherRepository userVoucherRepository, IMapper mapper)
            {
                _userVoucherRepository = userVoucherRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UserVoucherDto>> Handle(
                GetUserVouchersByUIdQuery request,
                CancellationToken cancellationToken)
            {
                var userVouchers = await _userVoucherRepository
                    .GetUserVouchersByUIdAsync(request.UserId, cancellationToken);

                return _mapper.Map<IEnumerable<UserVoucherDto>>(userVouchers);
            }
        }
    }
}
