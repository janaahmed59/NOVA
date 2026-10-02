using Microsoft.EntityFrameworkCore;
using NOVA.Domain.Entities;
using NOVAInfrastructure.Persistence;

namespace NOVAInfrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        var categories = new List<Category>
        {
            new()
            {
                Name = "Women",
                Description = "Clothing and accessories for women",
                ImageUrl = "https://example.com/women.jpg",
                IsActive = true
            },
            new()
            {
                Name = "Men",
                Description = "Clothing and accessories for men",
                ImageUrl = "https://example.com/men.jpg",
                IsActive = true
            },
            new()
            {
                Name = "Accessories",
                Description = "Fashion accessories",
                ImageUrl = "https://example.com/accessories.jpg",
                IsActive = true
            },
            new()
            {
                Name = "Bags",
                Description = "Bags and handbags",
                ImageUrl = "https://example.com/bags.jpg",
                IsActive = true
            },
            new()
            {
                Name = "Shoes",
                Description = "Shoes and footwear",
                ImageUrl = "https://example.com/shoes.jpg",
                IsActive = true
            }
        };

        foreach (var category in categories)
        {
            var exists = await context.Categories
                .AnyAsync(c => c.Name == category.Name);

            if (!exists)
            {
                context.Categories.Add(category);
            }
        }
        await SeedProductsAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        var now = DateTime.UtcNow;
        if (await context.Products.AnyAsync())
            return;

        var women = await context.Categories
            .FirstAsync(c => c.Name == "Women");

        var men = await context.Categories
            .FirstAsync(c => c.Name == "Men");

        var shoes = await context.Categories
            .FirstAsync(c => c.Name == "Shoes");

        var bags = await context.Categories
            .FirstAsync(c => c.Name == "Bags");

        var products = new List<Product>
    {
        new()
        {
            Name = "Basic T-Shirt",
            Description = "Classic cotton T-shirt",
            Price = 450m,
            CategoryId = women.Id,
            StockQuantity = 50,
            IsActive = true,
            UpdatedAt = now
        },

        new()
        {
            Name = "Summer Dress",
            Description = "Lightweight summer dress",
            Price = 900m,
            CategoryId = women.Id,
            StockQuantity = 25,
            IsActive = true,
            UpdatedAt = now
        },

        new()
        {
            Name = "Classic Shirt",
            Description = "Classic casual shirt",
            Price = 750m,
            CategoryId = men.Id,
            StockQuantity = 30,
            IsActive = true,
            UpdatedAt = now
        },

        new()
        {
            Name = "Casual Hoodie",
            Description = "Comfortable everyday hoodie",
            Price = 1100m,
            CategoryId = men.Id,
            StockQuantity = 20,
            IsActive = true,
            UpdatedAt = now
        },

        new()
        {
            Name = "White Sneakers",
            Description = "Classic everyday sneakers",
            Price = 1500m,
            CategoryId = shoes.Id,
            StockQuantity = 15,
            IsActive = true,
            UpdatedAt = now
        },

        new()
        {
            Name = "Leather Handbag",
            Description = "Elegant everyday handbag",
            Price = 1800m,
            CategoryId = bags.Id,
            StockQuantity = 10,
            IsActive = true,
            UpdatedAt = now
        }
    };

        await context.Products.AddRangeAsync(products);
    }
}