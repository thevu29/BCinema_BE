using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Vouchers.Queries;

public class GetVouchersQuery : IRequest<PaginatedList<VoucherDto>>
{
    public VoucherQuery Query { get; init; } = default!;

    public class GetVouchersQueryHandler(IVoucherRepository voucherRepository, IMapper mapper)
        : IRequestHandler<GetVouchersQuery, PaginatedList<VoucherDto>>
    {
        public async Task<PaginatedList<VoucherDto>> Handle(GetVouchersQuery request, CancellationToken cancellationToken)
        {
            var query = voucherRepository.GetVouchers();

            if (!string.IsNullOrEmpty(request.Query.Code))
            {
                query = query.Where(x => x.Code.ToLower().Contains(request.Query.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.Query.Discount))
            {
                query = query.FilterByNumber(request.Query.Discount, v => v.Discount);
            }

            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

            var vouchers = await PaginatedList<Voucher>
                .ToPageList(query, request.Query.Page, request.Query.Size);

            var voucherDtos = mapper.Map<IEnumerable<VoucherDto>>(vouchers.Data);

            return new PaginatedList<VoucherDto>(vouchers.Page, vouchers.Size, vouchers.TotalElements, voucherDtos);
        }
        
        private static IQueryable<Voucher> ApplySorting(IQueryable<Voucher> query, string sortBy, string sortOrder)
        {
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(Voucher.Code),
                nameof(Voucher.Discount),
                nameof(Voucher.CreateAt)
            };
            
            if (string.IsNullOrEmpty(sortBy) || !allowedSortColumns.Contains(sortBy))
            {
                return query.OrderByDescending(v => v.CreateAt);
            }

            return query.ApplyDynamicSorting(sortBy, sortOrder);
        }
    }
}