using ExpensesData.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesData.Context;
#pragma warning disable CS8618
public class ExpensesDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Outlay> Outlays { get; set; }

    public ExpensesDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        OutLaysConfiguration.Configure(modelBuilder.Entity<Outlay>());

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpensesDbContext).Assembly);
    }
}



