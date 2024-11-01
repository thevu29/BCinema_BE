using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Foods.Queries;

public class GetFoodByIdQuery : IRequest<FoodDto>
{
    public Guid Id { get; set; }

    public class GetFoodByIdQueryHandler : IRequestHandler<GetFoodByIdQuery, FoodDto>
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public GetFoodByIdQueryHandler(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
        {
            var food = await _foodRepository.GetFoodByIdAsync(request.Id, cancellationToken)
                       ?? throw new NotFoundException(nameof(Food));

            return _mapper.Map<FoodDto>(food);
        }
    }
}