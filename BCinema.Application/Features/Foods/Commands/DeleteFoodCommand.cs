using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Foods.Commands;

public class DeleteFoodCommand : IRequest
{
    public Guid Id { get; set; }

    public class DeleteFoodCommandHandler(IFoodRepository foodRepository) : IRequestHandler<DeleteFoodCommand>
    {
        public async Task Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetFoodByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Food");
            
            food.DeleteAt = DateTime.UtcNow;

            await foodRepository.SaveChangesAsync(cancellationToken);
        }
    }
}