using Microsoft.EntityFrameworkCore;
using AssetPrices.Api.Database;
using AssetPrices.Api.Endpoints;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace AssetPrices.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddFluentValidationAutoValidation();

            // Configure EF Core with In-Memory Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("AssetPricesDb"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Register endpoints
            app.MapAssetEndpoints();
            app.MapPriceEndpoints();

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var seedAssets = builder.Configuration.GetValue<bool>("DatabaseSettings:SeedAssets");
                DatabaseSeeder.Seed(dbContext, seedAssets);
            }

            app.Run();
        }
    }
}
