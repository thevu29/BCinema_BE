﻿using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Payments.Queries;

public class GetPaymentsQuery : IRequest<PaginatedList<PaymentDto>>
{
    public PaymentQuery Query { get; init; } = default!;
    
    public class GetPaymentsQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
        : IRequestHandler<GetPaymentsQuery, PaginatedList<PaymentDto>>
    {
        public async Task<PaginatedList<PaymentDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var query = paymentRepository.GetPayments();

            if (!string.IsNullOrEmpty(request.Query.Search))
            {
                var searchTerm = request.Query.Search.Trim().ToLower();    
                
                query = query.Where(x => 
                    EF.Functions.Like(x.User.Name.ToLower(), $"%{searchTerm}%") ||
                    EF.Functions.Like(x.Schedule.MovieName.ToLower(), $"%{searchTerm}%") ||
                    EF.Functions.Like(x.Voucher != null ? x.Voucher.Code.ToLower() : "", 
                        $"%{searchTerm}%")
                    );
            }
            if (request.Query.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == request.Query.UserId);
            }
            if (!string.IsNullOrEmpty(request.Query.Date))
            {
                query = query.FilterByDate(request.Query.Date, p => p.Date);
            }

            if (request.Query.HasPoint is true)
            {
                query = query.Where(x => x.Point != 0 && x.Point != null);
            }
            
            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);
            
            var payments = await PaginatedList<Payment>
                .ToPageList(query, request.Query.Page, request.Query.Size);

            var paymentDtos = mapper.Map<IEnumerable<PaymentDto>>(payments.Data);
            
            return new PaginatedList<PaymentDto>(request.Query.Page, request.Query.Size, payments.TotalElements, paymentDtos);
        }
        
        private static IQueryable<Payment> ApplySorting(IQueryable<Payment> query, string sortBy, string sortOrder)
        {
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(Payment.Date),
                nameof(Payment.TotalPrice)
            };
            
            if (string.IsNullOrWhiteSpace(sortBy) || !allowedSortColumns.Contains(sortBy))
            {
                return query.OrderByDescending(p => p.Date);
            }

            return query.ApplyDynamicSorting(sortBy, sortOrder);
        }
    }
}