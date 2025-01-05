using FlutterCounterService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FlutterCounterService.Infrastructure;

public class FlutterCounterServiceDbContext : DbContext
{
    public FlutterCounterServiceDbContext(DbContextOptions<FlutterCounterServiceDbContext> options)
        : base(options) { }

    public DbSet<CounterDbModel> Counters { get; set; }

    public DbSet<UserDbModel> Users { get; set; }
}
