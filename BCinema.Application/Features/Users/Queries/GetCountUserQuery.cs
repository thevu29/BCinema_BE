using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Users.Queries;

public class GetCountUserQuery : IRequest<int>
{
    public int Year { get; set; }
    public int Month { get; set;  }
    
    public class GetCountUserQueryHandler : IRequestHandler<GetCountUserQuery, int>
    {
        private readonly IUserRepository _userRepository;

        public GetCountUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(GetCountUserQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.CountAsync(request.Year, request.Month, cancellationToken);
        }
    }
}