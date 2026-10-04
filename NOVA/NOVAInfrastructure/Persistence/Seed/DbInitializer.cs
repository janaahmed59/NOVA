using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NOVA.Domain.Entities;
using NOVA.Domain.Enums;
using NOVAInfrastructure.Persistence;

namespace NOVAInfrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        await SeedCategoriesAsync(context);
        await context.SaveChangesAsync();

        await SeedUsersAsync(context);
        await context.SaveChangesAsync();

        await SeedAddressesAsync(context);
        await context.SaveChangesAsync();

        await SeedProductsAsync(context);
        await context.SaveChangesAsync();

        await SeedProductImagesAsync(context);
        await context.SaveChangesAsync();

        await SeedReviewsAsync(context);
        await context.SaveChangesAsync();

        await SeedOrdersAsync(context);
        await context.SaveChangesAsync();

        await SeedCartsAsync(context);
        await context.SaveChangesAsync();
    }

    // ──────────────────────────── Categories ────────────────────────────
    private static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        var categories = new List<Category>
        {
            new() { Name = "Women",       Description = "Clothing and accessories for women", ImageUrl = "https://example.com/women.jpg",       IsActive = true },
            new() { Name = "Men",         Description = "Clothing and accessories for men",   ImageUrl = "https://example.com/men.jpg",         IsActive = true },
            new() { Name = "Accessories", Description = "Fashion accessories",                ImageUrl = "https://example.com/accessories.jpg", IsActive = true },
            new() { Name = "Bags",        Description = "Bags and handbags",                  ImageUrl = "https://example.com/bags.jpg",        IsActive = true },
            new() { Name = "Shoes",       Description = "Shoes and footwear",                 ImageUrl = "https://example.com/shoes.jpg",       IsActive = true }
        };

        foreach (var category in categories)
        {
            var exists = await context.Categories.AnyAsync(c => c.Name == category.Name);
            if (!exists)
                context.Categories.Add(category);
        }
    }

    // ──────────────────────────── Users ────────────────────────────
    private static async Task SeedUsersAsync(ApplicationDbContext context)
    {
        var hasher = new PasswordHasher<User>();
        var now = DateTime.UtcNow;

        var users = new List<User>
        {
            new() { FullName = "Admin User",   Email = "admin@nova.com",    Role = GlobalRole.Admin, IsActive = true, CreatedAt = now },
            new() { FullName = "Ahmed Hassan", Email = "ahmed@example.com", Role = GlobalRole.User,  IsActive = true, CreatedAt = now },
            new() { FullName = "Sara Mohamed", Email = "sara@example.com",  Role = GlobalRole.User,  IsActive = true, CreatedAt = now }
        };

        foreach (var user in users)
        {
            var exists = await context.Users.AnyAsync(u => u.Email == user.Email);
            if (exists) continue;

            // PasswordHasher needs the user instance
            user.PasswordHash = hasher.HashPassword(user, "P@ssw0rd123");
            context.Users.Add(user);
        }
    }

    // ──────────────────────────── Addresses ────────────────────────────
    private static async Task SeedAddressesAsync(ApplicationDbContext context)
    {
        var ahmed = await context.Users.FirstAsync(u => u.Email == "ahmed@example.com");
        var sara = await context.Users.FirstAsync(u => u.Email == "sara@example.com");

        var addresses = new List<Address>
        {
            new() { UserId = ahmed.Id, Street = "15 El-Tahrir Street", City = "Cairo",      State = "Cairo",      ZipCode = "11511", IsDefault = true  },
            new() { UserId = ahmed.Id, Street = "22 Corniche Road",    City = "Alexandria", State = "Alexandria", ZipCode = "21599", IsDefault = false },
            new() { UserId = sara.Id,  Street = "8 Nile Avenue",       City = "Giza",       State = "Giza",       ZipCode = "12511", IsDefault = true  }
        };

        foreach (var address in addresses)
        {
            var exists = await context.Addresses
                .AnyAsync(a => a.UserId == address.UserId && a.Street == address.Street);

            if (!exists)
                context.Addresses.Add(address);
        }
    }

    // ──────────────────────────── Products ────────────────────────────
    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        var now = DateTime.UtcNow;

        var women = await context.Categories.FirstAsync(c => c.Name == "Women");
        var men = await context.Categories.FirstAsync(c => c.Name == "Men");
        var shoes = await context.Categories.FirstAsync(c => c.Name == "Shoes");
        var bags = await context.Categories.FirstAsync(c => c.Name == "Bags");
        var accessories = await context.Categories.FirstAsync(c => c.Name == "Accessories");

        var products = new List<Product>
        {
            new() { Name = "Basic T-Shirt",   Description = "Classic cotton T-shirt",                         Price = 450m,  CategoryId = women.Id,       StockQuantity = 50, IsActive = true, UpdatedAt = now },
            new() { Name = "Summer Dress",    Description = "Lightweight summer dress",                       Price = 900m,  CategoryId = women.Id,       StockQuantity = 25, IsActive = true, UpdatedAt = now },
            new() { Name = "Classic Shirt",   Description = "Classic casual shirt",                           Price = 750m,  CategoryId = men.Id,         StockQuantity = 30, IsActive = true, UpdatedAt = now },
            new() { Name = "Casual Hoodie",   Description = "Comfortable everyday hoodie",                    Price = 1100m, CategoryId = men.Id,         StockQuantity = 20, IsActive = true, UpdatedAt = now },
            new() { Name = "White Sneakers",  Description = "Classic everyday sneakers",                      Price = 1500m, CategoryId = shoes.Id,       StockQuantity = 15, IsActive = true, UpdatedAt = now },
            new() { Name = "Leather Handbag", Description = "Elegant everyday handbag",                       Price = 1800m, CategoryId = bags.Id,        StockQuantity = 10, IsActive = true, UpdatedAt = now },
            new() { Name = "Running Shoes",   Description = "Lightweight running shoes with cushion support", Price = 1350m, CategoryId = shoes.Id,       StockQuantity = 18, IsActive = true, UpdatedAt = now },
            new() { Name = "Silver Bracelet", Description = "Elegant sterling silver bracelet",               Price = 650m,  CategoryId = accessories.Id, StockQuantity = 40, IsActive = true, UpdatedAt = now },
            new() { Name = "Sunglasses",      Description = "UV-protected stylish sunglasses",                Price = 500m,  CategoryId = accessories.Id, StockQuantity = 35, IsActive = true, UpdatedAt = now },
            new() { Name = "Denim Jacket",    Description = "Classic blue denim jacket",                      Price = 1250m, CategoryId = men.Id,         StockQuantity = 12, IsActive = true, UpdatedAt = now }
        };

        foreach (var product in products)
        {
            var exists = await context.Products.AnyAsync(p => p.Name == product.Name);
            if (!exists)
                context.Products.Add(product);
        }
    }

    // ──────────────────────────── Product Images ────────────────────────────
    private static async Task SeedProductImagesAsync(ApplicationDbContext context)
    {
        // Only seed images for the seeded products (not products created from the API)
        var seededNames = new[]
        {
            "Basic T-Shirt", "Summer Dress", "Classic Shirt", "Casual Hoodie", "White Sneakers",
            "Leather Handbag", "Running Shoes", "Silver Bracelet", "Sunglasses", "Denim Jacket"
        };

        var products = await context.Products
            .Where(p => seededNames.Contains(p.Name!))
            .ToListAsync();

        foreach (var product in products)
        {
            var slug = product.Name!.ToLower().Replace(" ", "-");

            var mainUrl = $"https://example.com/products/{slug}-main.jpg";
            var secondaryUrl = $"https://example.com/products/{slug}-2.jpg";

            if (!await context.ProductImages.AnyAsync(i => i.ProductId == product.Id && i.ImageUrl == mainUrl))
            {
                context.ProductImages.Add(new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = mainUrl,
                    IsMain = true
                });
            }

            if (!await context.ProductImages.AnyAsync(i => i.ProductId == product.Id && i.ImageUrl == secondaryUrl))
            {
                context.ProductImages.Add(new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = secondaryUrl,
                    IsMain = false
                });
            }
        }
    }

    // ──────────────────────────── Reviews ────────────────────────────
    private static async Task SeedReviewsAsync(ApplicationDbContext context)
    {
        var now = DateTime.UtcNow;
        var ahmed = await context.Users.FirstAsync(u => u.Email == "ahmed@example.com");
        var sara = await context.Users.FirstAsync(u => u.Email == "sara@example.com");

        var tShirt = await context.Products.FirstAsync(p => p.Name == "Basic T-Shirt");
        var dress = await context.Products.FirstAsync(p => p.Name == "Summer Dress");
        var hoodie = await context.Products.FirstAsync(p => p.Name == "Casual Hoodie");
        var sneakers = await context.Products.FirstAsync(p => p.Name == "White Sneakers");
        var handbag = await context.Products.FirstAsync(p => p.Name == "Leather Handbag");

        var reviews = new List<Review>
        {
            new() { ProductId = tShirt.Id,   UserId = ahmed.Id, Rating = 5, Comment = "Excellent quality cotton, very comfortable!", CreatedAt = now.AddDays(-10) },
            new() { ProductId = tShirt.Id,   UserId = sara.Id,  Rating = 4, Comment = "Nice fit and good material.",                 CreatedAt = now.AddDays(-8)  },
            new() { ProductId = dress.Id,    UserId = sara.Id,  Rating = 5, Comment = "Beautiful dress, perfect for summer!",       CreatedAt = now.AddDays(-7)  },
            new() { ProductId = hoodie.Id,   UserId = ahmed.Id, Rating = 4, Comment = "Very warm and cozy, great for winter.",      CreatedAt = now.AddDays(-5)  },
            new() { ProductId = sneakers.Id, UserId = ahmed.Id, Rating = 5, Comment = "Best sneakers I've ever owned!",             CreatedAt = now.AddDays(-3)  },
            new() { ProductId = sneakers.Id, UserId = sara.Id,  Rating = 3, Comment = "Good quality but a bit tight.",              CreatedAt = now.AddDays(-2)  },
            new() { ProductId = handbag.Id,  UserId = sara.Id,  Rating = 5, Comment = "Gorgeous bag, love the leather quality!",    CreatedAt = now.AddDays(-1)  }
        };

        foreach (var review in reviews)
        {
            var exists = await context.Reviews
                .AnyAsync(r => r.ProductId == review.ProductId && r.UserId == review.UserId);

            if (!exists)
                context.Reviews.Add(review);
        }
    }

    // ──────────────────────────── Orders + Order Items ────────────────────────────
    private static async Task SeedOrdersAsync(ApplicationDbContext context)
    {
        var now = DateTime.UtcNow;
        var ahmed = await context.Users.FirstAsync(u => u.Email == "ahmed@example.com");
        var sara = await context.Users.FirstAsync(u => u.Email == "sara@example.com");

        var ahmedAddress = await context.Addresses.FirstAsync(a => a.UserId == ahmed.Id && a.IsDefault);
        var saraAddress = await context.Addresses.FirstAsync(a => a.UserId == sara.Id && a.IsDefault);

        var tShirt = await context.Products.FirstAsync(p => p.Name == "Basic T-Shirt");
        var hoodie = await context.Products.FirstAsync(p => p.Name == "Casual Hoodie");
        var dress = await context.Products.FirstAsync(p => p.Name == "Summer Dress");
        var sneakers = await context.Products.FirstAsync(p => p.Name == "White Sneakers");
        var handbag = await context.Products.FirstAsync(p => p.Name == "Leather Handbag");

        var orders = new List<Order>
        {
            // Order 1 — Ahmed — Delivered
            new()
            {
                UserId = ahmed.Id,
                AddressId = ahmedAddress.Id,
                TotalAmount = (tShirt.Price * 2) + hoodie.Price,   // 450*2 + 1100 = 2000
                Status = OrderStatus.Delivered,
                CreatedAt = now.AddDays(-15),
                OrderItems = new List<OrderItem>
                {
                    new() { ProductId = tShirt.Id, ProductName = tShirt.Name, Quantity = 2, UnitPrice = tShirt.Price },
                    new() { ProductId = hoodie.Id, ProductName = hoodie.Name, Quantity = 1, UnitPrice = hoodie.Price }
                }
            },

            // Order 2 — Sara — Shipped
            new()
            {
                UserId = sara.Id,
                AddressId = saraAddress.Id,
                TotalAmount = dress.Price + handbag.Price,   // 900 + 1800 = 2700
                Status = OrderStatus.Shipped,
                CreatedAt = now.AddDays(-5),
                OrderItems = new List<OrderItem>
                {
                    new() { ProductId = dress.Id,   ProductName = dress.Name,   Quantity = 1, UnitPrice = dress.Price   },
                    new() { ProductId = handbag.Id, ProductName = handbag.Name, Quantity = 1, UnitPrice = handbag.Price }
                }
            },

            // Order 3 — Ahmed — Pending
            new()
            {
                UserId = ahmed.Id,
                AddressId = ahmedAddress.Id,
                TotalAmount = sneakers.Price,   // 1500
                Status = OrderStatus.Pending,
                CreatedAt = now.AddDays(-1),
                OrderItems = new List<OrderItem>
                {
                    new() { ProductId = sneakers.Id, ProductName = sneakers.Name, Quantity = 1, UnitPrice = sneakers.Price }
                }
            }
        };

        foreach (var order in orders)
        {
            // Orders have no natural key, so we match on user + status + total
            var exists = await context.Orders.AnyAsync(o =>
                o.UserId == order.UserId &&
                o.Status == order.Status &&
                o.TotalAmount == order.TotalAmount);

            if (!exists)
                context.Orders.Add(order);
        }
    }

    // ──────────────────────────── Carts + Cart Items ────────────────────────────
    private static async Task SeedCartsAsync(ApplicationDbContext context)
    {
        var sara = await context.Users.FirstAsync(u => u.Email == "sara@example.com");
        var bracelet = await context.Products.FirstAsync(p => p.Name == "Silver Bracelet");
        var sunglasses = await context.Products.FirstAsync(p => p.Name == "Sunglasses");

        var cart = await context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == sara.Id);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = sara.Id,
                CartItems = new List<CartItem>()
            };
            context.Carts.Add(cart);
        }

        if (!cart.CartItems.Any(i => i.ProductId == bracelet.Id))
            cart.CartItems.Add(new CartItem { ProductId = bracelet.Id, Quantity = 1 });

        if (!cart.CartItems.Any(i => i.ProductId == sunglasses.Id))
            cart.CartItems.Add(new CartItem { ProductId = sunglasses.Id, Quantity = 2 });
    }
}