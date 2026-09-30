using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Address.Common;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Address.Query
{
    public class GetUserAddressesQueryHandler : IRequestHandler<GetUserAddressesQuery, Result<List<AddressResponse>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public GetUserAddressesQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<List<AddressResponse>>> Handle(GetUserAddressesQuery request, CancellationToken ct)
        {
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<List<AddressResponse>>.Failure(NovaErrors.Unauthenticated);
            }

            var addresses = await _context.Addresses.AsNoTracking()
                .Where(a => a.UserId == userId.Value)
                .OrderByDescending(a => a.IsDefault) // العنوان الافتراضي أولاً
                .ThenByDescending(a => a.Id)
                .Select(a => new AddressResponse
                {
                    Id = a.Id,
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    IsDefault = a.IsDefault
                })
                .ToListAsync(ct);

            return Result<List<AddressResponse>>.Success(addresses);
        }
    }
}