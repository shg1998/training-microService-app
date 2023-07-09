﻿using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext catalogContext) =>
            this._context = catalogContext;

        public async Task CreateProduct(Product product) => await this._context.Products.InsertOneAsync(product);

        public async Task<bool> DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var deleteResult = await this._context.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await this._context.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await this._context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await this._context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts() =>
            await this._context.Products.Find(p => true).ToListAsync();

        public async Task<Product> ProductById(string id) => await this._context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

    }
}
