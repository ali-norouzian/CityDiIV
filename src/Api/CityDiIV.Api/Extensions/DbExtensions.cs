using CityDiIV.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CityDiIV.Framework.Extensions;
public static class DbExtensions
{
    public static async Task<IApplicationBuilder> UpdateDb(this IApplicationBuilder app)
    {
        await using (var scope = app.ApplicationServices.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetService<AppWriteDbContext>() ??
                           throw new Exception("Database Context Not Found");
            try
            {
                dbContext.Database.MigrateAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
            }
        }

        return app;
    }
}

