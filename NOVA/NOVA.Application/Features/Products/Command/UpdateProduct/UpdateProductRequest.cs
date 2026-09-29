using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.Products.Command.UpdateProduct
{
    public record UpdateProductRequest(
        string Name,
        string? Description,
        decimal Price,
        int CategoryId,
        int StockQuantity
    );
}
