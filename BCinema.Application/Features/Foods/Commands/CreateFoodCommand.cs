using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BCinema.Application.Features.Foods.Commands;

public class CreateFoodCommand : IRequest<FoodDto>
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public int Quantity { get; set; }
    public IFormFile Image { get; set; } = default!;

    public class CreateFoodCommandHandler(
        IFoodRepository foodRepository,
        IMapper mapper,
        IFileStorageService fileStorageService) : IRequestHandler<CreateFoodCommand, FoodDto>
    {
        public async Task<FoodDto> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
        {
            var food = mapper.Map<Food>(request);

            await using var imageStream = request.Image.OpenReadStream();
            food.Image = await fileStorageService.UploadImageAsync(
                imageStream, Guid.NewGuid() + ".jpg");

            await foodRepository.AddAsync(food, cancellationToken);
            await foodRepository.SaveChangesAsync(cancellationToken);

            return mapper.Map<FoodDto>(food);
        }
    }
}