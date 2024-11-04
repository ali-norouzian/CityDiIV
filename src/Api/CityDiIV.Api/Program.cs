using CityDiIV.Application;
using CityDiIV.Framework.Extensions;
using CityDiIV.Persistence;
using Microsoft.AspNetCore.Http.Features;

namespace CityDiIV.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        // Configure Kestrel server to handle large file uploads
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Limits.MaxRequestBodySize = 1 * 1024 * 1024 * 1024; // 1 GB
        });
        // Configure form options to support large multipart body sizes
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1 * 1024 * 1024 * 1024; // 1 GB
            options.ValueLengthLimit = 1 * 1024 * 1024 * 1024;
            options.MultipartHeadersLengthLimit = 1 * 1024 * 1024 * 1024;

        });

        // Add services to the container.
        builder.Services.AddPersistence()
                        .AddApplication();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        await app.UpdateDb();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        //app.UseAuthorization();

        app.UseRouting();
        app.MapControllers();

        await app.RunAsync();
    }
}