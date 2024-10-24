using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Roles.Queries;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.Features.Foods.Queries
{
    public class GetFoodsQuery : IRequest<IEnumerable<FoodDto>>
    {
        public class GetFoodsQueryHandler : IRequestHandler<GetFoodsQuery, IEnumerable<FoodDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetFoodsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<FoodDto>> Handle(GetFoodsQuery request, CancellationToken cancellationToken)
            {
                var roles = await _context.Foods.ToListAsync(cancellationToken);
                return _mapper.Map<IEnumerable<FoodDto>>(roles);
            }
        }
    }
}
