using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.Features.Foods.Commands
{
    public class DeleteFoodCommand : IRequest<FoodDto>
    {
        public Guid Id { get; set; }
        public DateTime DeleteAt { get; set; } = DateTime.Now;

        public class DeleteFoodCommandHandler : IRequestHandler<DeleteFoodCommand, FoodDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public DeleteFoodCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FoodDto> Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
            {
                var food = await _context.Foods.FindAsync(request.Id)
                                ?? throw new NotFoundException(nameof(Food), request.Id);

                food.DeleteAt = request.DeleteAt;
                _mapper.Map(request, food);
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<FoodDto>(food);
            }
        }
    }
}
