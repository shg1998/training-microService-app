using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost host, int? retry = 0)
        {
            var retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<T>>();
                try
                {
                    logger.LogInformation("Migrating Database ... !");
                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, ProductName VARCHAR(200) NOT NULL, Description TEXT, Amount INT)";
                    command.ExecuteNonQuery();

                    // seed DATA: 
                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Iphone x','Goolakh Phone!!',254)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Samsung Gallaxy S23 ultra','Such a Wow!!',105)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("migration has been completed!");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError("An Error has been occured!!!");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2050);
                        MigrateDatabase<T>(host, retryForAvailability);
                    }
                }
            }
            return host;
        }
    }
}
