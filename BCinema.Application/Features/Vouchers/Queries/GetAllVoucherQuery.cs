using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.Vouchers.Queries;

public class GetAllVoucherQuery : IRequest<PaginatedList<VoucherDto>>
{
    public VoucherQuery Query { get; set; } = default!;

    public class GetAllVoucherQueryHandler : IRequestHandler<GetAllVoucherQuery, PaginatedList<VoucherDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllVoucherQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<VoucherDto>> Handle(GetAllVoucherQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Voucher> query = _context.Vouchers;
            if (!string.IsNullOrEmpty(request.Query.Code))
            {
                query = query.Where(x => x.Code.ToLower().Contains(request.Query.Code.ToLower()));
            }

            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

            var vouchers = await PaginatedList<Voucher>
                .ToPageList(query, request.Query.Page, request.Query.Size);
            var voucherDtos = _mapper.Map<IEnumerable<VoucherDto>>(vouchers.Data);
            return new PaginatedList<VoucherDto>(vouchers.Page, vouchers.Size, vouchers.TotalElements, voucherDtos);
        }
        
        private static IQueryable<Voucher> ApplySorting(IQueryable<Voucher> query, string sortBy, string sortOrder)
        {
            switch (sortBy.ToLower())
            {
                case "createdat":
                    query = sortOrder.ToUpper() == "ASC"
                        ? query.OrderBy(v => v.CreateAt)
                        : query.OrderByDescending(v => v.CreateAt);
                    break;
                default:
                    query = sortOrder.ToUpper() == "ASC"
                        ? query.OrderBy(v => v.Id)
                        : query.OrderByDescending(v => v.Id);
                    break;
            }

            return query;
        }
    }
}