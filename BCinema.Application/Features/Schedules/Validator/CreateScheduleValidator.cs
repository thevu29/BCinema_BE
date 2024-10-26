using BCinema.Application.Features.Schedules.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Schedules.Validator
{
    public class CreateScheduleValidator : AbstractValidator<CreateScheduleCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateScheduleValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.MovieId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(MovieExists).WithMessage("{PropertyName} does not exist.");

            RuleFor(x => x.RoomId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(RoomExists).WithMessage("{PropertyName} does not exist.");
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .GreaterThan(DateTime.Now).WithMessage("{PropertyName} must be greater than today.")
                .MustAsync(ReleaseDateBeforeSchedule).WithMessage("The movie release date must be before the schedule date.")
                .MustAsync(NoOverLappingSchedule).WithMessage("There is an overlapping schedule in this room.");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("{PropertyName} is not valid.")
                .NotNull();
        }

        private async Task<bool> RoomExists(Guid roomId, CancellationToken cancellationToken)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == roomId, cancellationToken);
        }

        private async Task<bool> MovieExists(string movieId, CancellationToken cancellationToken)
        {
            return await _context.Movies.AnyAsync(m => m.Id == movieId, cancellationToken);
        }

        private async Task<bool> NoOverLappingSchedule(CreateScheduleCommand command, DateTime date, CancellationToken cancellationToken)
        {
            return !await _context.Schedules.AnyAsync(s =>
                s.RoomId == command.RoomId &&
                ((s.Date <= date && date <= s.Date.AddMinutes(s.Movie.Runtime + 15)) ||
                 (s.Date.AddMinutes(s.Movie.Runtime + 15) >= date && date >= s.Date)), cancellationToken);
        }

        private async Task<bool> ReleaseDateBeforeSchedule(CreateScheduleCommand command, DateTime date, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == command.MovieId, cancellationToken);
            if (movie == null)
            {
                return false;
            }

            return movie.ReleaseDate <= DateOnly.FromDateTime(date);
        }
    }
}
