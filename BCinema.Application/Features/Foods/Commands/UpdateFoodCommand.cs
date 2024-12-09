using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BCinema.Application.Features.Foods.Commands;

public class UpdateFoodCommand : IRequest<FoodDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }
    public IFormFile? Image { get; set; }

    public class UpdateFoodCommandHandler(
        IFoodRepository foodRepository,
        IFileStorageService fileStorageService,
        IMapper mapper) : IRequestHandler<UpdateFoodCommand, FoodDto>
    {
        public async Task<FoodDto> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetFoodByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Food));
            
            if (request.Image != null)
            {
                await using var imageStream = request.Image.OpenReadStream();
                
                food.Image = !string.IsNullOrEmpty(food.Image) 
                    ? await fileStorageService.UpdateImageAsync(imageStream, Guid.NewGuid() + ".jpg", food.Image)
                    : await fileStorageService.UploadImageAsync(imageStream, Guid.NewGuid() + ".jpg");
            }

            mapper.Map(request, food);
            
            await foodRepository.SaveChangesAsync(cancellationToken);

            return mapper.Map<FoodDto>(food);
        }
    }
}