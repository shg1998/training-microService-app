using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDb<T>(this IHost host, Action<T, IServiceProvider> seeder, int? retry = 0) where T : DbContext
        {
            int retry4Availability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<T>>();
                var context = services.GetService<T>();
                try
                {
                    logger.LogInformation("migrating started");
                    context.Database.Migrate();
                    seeder(context, services);
                    logger.LogInformation("migrating Done");
                }
                catch (SqlException e)
                {
                    throw;
                }
            }
            return host;
        }
    }
}
