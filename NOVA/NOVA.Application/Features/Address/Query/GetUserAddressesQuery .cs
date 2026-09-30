using MediatR;
using NOVA.Application.Features.Address.Common;
using NOVA.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.Address.Query
{
    public record GetUserAddressesQuery : IRequest<Result<List<AddressResponse>>>;

}
