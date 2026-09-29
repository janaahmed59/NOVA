using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext context;
        public DeleteCategoryCommandHandler(IApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken ct)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, ct);
            if (category is null)
            {
                return Result<Unit>.Failure(NovaErrors.NotFound);
            }
            category.IsActive = false;

            await context.SaveChangesAsync(ct);

            return Result<Unit>.Success(Unit.Value);

        }
    }
}
