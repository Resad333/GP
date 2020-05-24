using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Registration.Core.Entity;
using System.IO;

namespace Registration.Core.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
    {
        public CustomerDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<CustomerDbContext>();
            var connectionString = configuration.GetConnectionString("CustomerDbConnection");
            builder.UseSqlServer(connectionString);
            return new CustomerDbContext(builder.Options);
        }
    }


    public class CustomerDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public CustomerDbContext()
        {

        }
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();
    }
}
