using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.Foods.Commands
{
    public class UpdateFoodCommand : IRequest<FoodDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
        public double Price { get; set; }

        public class UpdateFoodCommandHandler: IRequestHandler<UpdateFoodCommand,
            FoodDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public UpdateFoodCommandHandler(IApplicationDbContext context,
                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FoodDto> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
            {
                var food = await _context.Foods.FindAsync(request.Id)
                                ?? throw new NotFoundException(nameof(Food),
                                request.Id);

                _mapper.Map(request, food);
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<FoodDto>(food);
            }
        }
    }
}
