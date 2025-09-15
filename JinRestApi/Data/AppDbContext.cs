using Microsoft.EntityFrameworkCore;
using JinRestApi.Models;

namespace JinRestApi.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}

