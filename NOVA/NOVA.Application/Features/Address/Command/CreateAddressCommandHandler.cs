using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Address.Common;
using NOVA.Domain.Common.Results;
//using NOVA.Domain.Entities;
namespace NOVA.Application.Features.Address.Command
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Result<AddressResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public CreateAddressCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken ct)
        {
            // 1. التحقق من المستخدم الحالي
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<AddressResponse>.Failure(NovaErrors.Unauthenticated);
            }

            // 2. التحقق هل يمتلك المستخدم أي عناوين سابقة؟
            var hasAnyAddress = await _context.Addresses.AnyAsync(a => a.UserId == userId.Value, ct);

            // لو ده أول عنوان له، نجعله افتراضياً تلقائياً، أو إذا اختار بنفسه أن يكون Default
            var isDefault = !hasAnyAddress || request.IsDefault;

            // إذا كان هذا العنوان سيكون Default، نلغي الـ Default عن بقية العناوين السابقة
            if (isDefault && hasAnyAddress)
            {
                var existingDefaults = await _context.Addresses
                    .Where(a => a.UserId == userId.Value && a.IsDefault)
                    .ToListAsync(ct);

                foreach (var addr in existingDefaults)
                {
                    addr.IsDefault = false;
                }
            }

            // 3. إنشاء العنوان وحفظه
            var address = new NOVA.Domain.Entities.Address
            {
                UserId = userId.Value,
                Street = request.Street,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                IsDefault = isDefault
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync(ct);

            // 4. تجهيز الـ Response
            var response = new AddressResponse
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode,
                IsDefault = address.IsDefault
            };

            return Result<AddressResponse>.Success(response);
        }
    }
}