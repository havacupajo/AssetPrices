using AssetPrices.Api.Contracts;
using AssetPrices.Api.Database;
using AssetPrices.Api.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetPrices.Api.Endpoints
{
    public static class PriceHandlers
    {
        // Handler for retrieving prices for one or more assets
        public static async Task<IResult> GetPrices([FromQuery] string? symbol, [FromQuery] DateOnly? date, [FromQuery] string? source, ApplicationDbContext dbContext)
        {
            var query = dbContext.AssetPrices.AsQueryable();

            if (!string.IsNullOrEmpty(symbol))
            {
                query = query.Where(p => p.Symbol == symbol);
            }

            if (date.HasValue)
            {
                query = query.Where(p => p.Date == date.Value);
            }

            if (!string.IsNullOrEmpty(source))
            {
                query = query.Where(p => p.Source == source);
            }

            var prices = await query.ToListAsync();
            return Results.Ok(prices);
        }

        // Handler for creating a new price
        public static async Task<IResult> CreatePrice([FromBody] AssetPriceDto priceDto, IValidator<AssetPriceDto> validator, ApplicationDbContext dbContext)
        {
            var validationResult = await validator.ValidateAsync(priceDto);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var existingPrice = await dbContext.AssetPrices.FirstOrDefaultAsync(p =>
                p.Symbol == priceDto.Symbol &&
                p.Date == priceDto.Date &&
                p.Source == priceDto.Source);

            if (existingPrice is not null)
            {
                return Results.Conflict($"A price for the asset '{priceDto.Symbol}' from source '{priceDto.Source}' on date '{priceDto.Date}' already exists.");
            }

            var price = new AssetPrice
            {
                Symbol = priceDto.Symbol,
                Source = priceDto.Source,
                Date = priceDto.Date,
                Price = priceDto.Price,
                LastUpdated = DateTime.UtcNow
            };

            await dbContext.AssetPrices.AddAsync(price);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/prices/{price.Symbol}/{price.Date}/{price.Source}", priceDto);
        }

        // Handler for updating an existing price
        public static async Task<IResult> UpdatePrice([FromBody] AssetPriceDto priceDto, IValidator<AssetPriceDto> validator, ApplicationDbContext dbContext)
        {
            var validationResult = await validator.ValidateAsync(priceDto);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var existingPrice = await dbContext.AssetPrices.FirstOrDefaultAsync(p =>
                p.Symbol == priceDto.Symbol &&
                p.Date == priceDto.Date &&
                p.Source == priceDto.Source);

            if (existingPrice is null)
            {
                return Results.NotFound($"No price found for the asset '{priceDto.Symbol}' from source '{priceDto.Source}' on date '{priceDto.Date}'.");
            }

            existingPrice.Price = priceDto.Price;
            existingPrice.LastUpdated = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return Results.Ok(priceDto);
        }
    }
}
