using MediatR;
using NOVA.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.Categories.Commands.Create_Category
{
    public record CreateCategoryCommand(
        string Name,
        string Description,
        string IamgeUrl,
        bool IsActive) : IRequest<Result<CreateCategoryResponse>>;
    
}
