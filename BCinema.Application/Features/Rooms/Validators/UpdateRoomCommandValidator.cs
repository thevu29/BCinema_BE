using BCinema.Application.Features.Rooms.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Rooms.Validators;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    
    public UpdateRoomCommandValidator(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
        
        RuleFor(x => x.Name)
            .Must(desc => desc == null || !string.IsNullOrEmpty(desc)).WithMessage("Name cannot be empty")
            .MustAsync(BeUniqueName).WithMessage("Room already exists");

        RuleFor(x => x.Description)
            .Must(desc => desc == null || !string.IsNullOrEmpty(desc)).WithMessage("Description cannot be empty");
    }
    
    private async Task<bool> BeUniqueName(
        UpdateRoomCommand command,
        string? name,
        CancellationToken cancellationToken)
    {
        return !await _roomRepository.AnyAsync(r => r.Name == name && r.Id != command.Id, cancellationToken);
    }
}