using Microsoft.EntityFrameworkCore;

namespace Warehouse.Models {

    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ProductLine> ProductLines { get; set; }
    }
}
