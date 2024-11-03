using Microsoft.EntityFrameworkCore;

namespace CityDiIV.Persistence;

public class AppWriteDbContext : AppDbContext
{
    public AppWriteDbContext(DbContextOptions options) : base(options)
    {
    }
}

