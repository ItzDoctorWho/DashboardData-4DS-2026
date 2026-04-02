using Microsoft.EntityFrameworkCore;
using DashboardData.Models;

namespace DashboardData.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Chaque DbSet = une Table SQL
    public DbSet<SensorData> Sensors { get; set; }
    public DbSet<Location> Locations { get; set; }  // NOUVEAU
    public DbSet<Tag> Tags { get; set; }              // NOUVEAU
    public DbSet<SensorValueHistory> SensorValueHistories { get; set; }
}