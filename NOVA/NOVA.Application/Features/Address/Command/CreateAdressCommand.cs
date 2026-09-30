using MediatR;
using NOVA.Application.Features.Address.Common;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Address.Command
{
    public record CreateAddressCommand(
        string Street,
        string City,
        string? State,
        string? ZipCode,
        bool IsDefault = false
    ) : IRequest<Result<AddressResponse>>;
}