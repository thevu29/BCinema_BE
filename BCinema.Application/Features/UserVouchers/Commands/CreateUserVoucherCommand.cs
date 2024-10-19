using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.UserVouchers.Commands;

public class CreateUserVoucherCommand : IRequest<UserVoucherDto>
{
    public Guid UserId { get; set; }
    public Guid VoucherId { get; set; }
    
    public class CreateUserVoucherCommandHandler : IRequestHandler<CreateUserVoucherCommand, UserVoucherDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserVoucherCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<UserVoucherDto> Handle(CreateUserVoucherCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId)
                ?? throw new NotFoundException(nameof(User), request.UserId);
            var voucher = await _context.Vouchers.FindAsync(request.VoucherId)
                ?? throw new NotFoundException(nameof(Voucher), request.VoucherId);
            if (voucher.ExpireAt < DateTime.UtcNow)
            {
                throw new BadRequestException("Voucher is expired.");
            }
            _context.Vouchers.Update(voucher);
            var userVoucher = _mapper.Map<UserVoucher>(request);
            _context.UserVouchers.Add(userVoucher);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserVoucherDto>(userVoucher);
        }
    }
}