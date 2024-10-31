using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Users.Commands;

public class DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        
        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(User));

            user.DeleteAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync(cancellationToken);
        }
    }
}