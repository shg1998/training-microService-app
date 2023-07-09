using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DbSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DbSettings:DatabaseName"));
            this.Products = database.GetCollection<Product>(configuration.GetValue<string>("DbSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }
    }
}
