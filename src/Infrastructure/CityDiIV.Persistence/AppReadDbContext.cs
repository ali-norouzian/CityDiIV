using Microsoft.EntityFrameworkCore;

namespace CityDiIV.Persistence;

public class AppReadDbContext : AppDbContext
{
    public AppReadDbContext(DbContextOptions options) : base(options)
    {
    }
}

