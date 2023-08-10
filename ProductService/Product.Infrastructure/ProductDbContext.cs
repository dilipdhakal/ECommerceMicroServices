using Microsoft.EntityFrameworkCore;
using Prodcut.Domain.Entities;

namespace Product.Infrastructure
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        { }
        public DbSet<Products> products { get; set; }
    }
}
