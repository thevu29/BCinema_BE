using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;

namespace BCinema.Application.Features.Payments.Queries;

public class GetTopMoviesMostViewedQuery : IRequest<IEnumerable<MovieDto>>
{
    public int Count { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    
    public class GetTopMoviesMostViewedQueryHandler(
        IPaymentRepository paymentRepository,
        IMovieFetchService movieFetchService)
        : IRequestHandler<GetTopMoviesMostViewedQuery, IEnumerable<MovieDto>>
    {
        public async Task<IEnumerable<MovieDto>> Handle(GetTopMoviesMostViewedQuery request, CancellationToken cancellationToken)
        {
            var topMovies = await paymentRepository
                .GetTopMoviesMostViewedAsync(request.Year,request.Month,request.Count, cancellationToken) as List<TopMovieDto>;
            var movieDtos = new List<MovieDto>();
            
            Console.WriteLine("Top Movies: " + topMovies);

            if (topMovies == null)
            {
                return movieDtos;
            }
            
            foreach (var movie in topMovies)
            {
                Console.WriteLine("Top Movies: " + movie.TotalView);
                var movieDto = await movieFetchService.FetchMovieByIdAsync(movie.MovieId);
                movieDtos.Add(movieDto);
            }

            return movieDtos;
        }
    }
}