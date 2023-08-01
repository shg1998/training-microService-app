using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderDbContextSeed
    {
        public static async Task SeedAsync(OrderDbContext dbContext, ILogger<OrderDbContextSeed> logger)
        {
            if (!await dbContext.Orders.AnyAsync())
            {
                await dbContext.Orders.AddRangeAsync(GetPreConfigOrders());
                await dbContext.SaveChangesAsync();
                logger.LogInformation("data Seeded!");
            }
        }

        private static IEnumerable<Order> GetPreConfigOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    FirstName = "MohammadHossein",
                    LastName = "Nejadhendi",
                    UserName= "mohammad",
                    EmailAddress="djklhdkhd@khfdkhd.hdkhd",
                    City= "Tehran",
                    Country= "iran",
                    TotalPrice = 10000
                }
            };
        }
    }
}
