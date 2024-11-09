using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Users.Commands;

public class DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }

    public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(User));
            
            if (user.Role.Name.ToLower().Equals("admin"))
            {
                throw new BadRequestException("Admin cannot be deleted");
            }

            user.DeleteAt = DateTime.UtcNow;

            await userRepository.SaveChangesAsync(cancellationToken);
        }
    }
}