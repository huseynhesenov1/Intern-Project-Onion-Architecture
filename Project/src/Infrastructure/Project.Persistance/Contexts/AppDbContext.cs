using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;

namespace Project.Persistance.Contexts;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions opt) : base(opt) { }
     
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductDistrictPrice> ProductDistrictPrices { get; set; }
}