using AssetPrices.Api.Database;
using AssetPrices.Api.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetPrices.Api.Endpoints
{
    public static class AssetHandlers
    {
        // Handler for retrieving all assets
        public static async Task<IResult> GetAllAssets(ApplicationDbContext dbContext)
        {
            var allAssets = await dbContext.Assets.ToListAsync();
            return Results.Ok(allAssets);
        }

        // Handler for retrieving a specific asset by symbol
        public static async Task<IResult> GetAssetBySymbol([FromRoute] string symbol, ApplicationDbContext dbContext)
        {
            var asset = await dbContext.Assets
                .Where(a => a.Symbol == symbol)
                .FirstOrDefaultAsync();

            return asset is not null
                ? Results.Ok(asset)
                : Results.NotFound($"No asset found with the symbol '{symbol}'.");
        }

        // Handler for creating a new asset
        public static async Task<IResult> CreateAsset([FromBody] Asset asset, IValidator<Asset> validator, ApplicationDbContext dbContext)
        {
            var validationResult = await validator.ValidateAsync(asset);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var existingAsset = await dbContext.Assets.FirstOrDefaultAsync(a => a.Symbol == asset.Symbol);
            if (existingAsset is not null)
            {
                return Results.Conflict($"An asset with the symbol '{asset.Symbol}' already exists.");
            }

            await dbContext.Assets.AddAsync(asset);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/assets/{asset.Symbol}", asset);
        }

        // Handler for updating an existing asset
        public static async Task<IResult> UpdateAsset([FromBody] Asset asset, IValidator<Asset> validator, ApplicationDbContext dbContext)
        {
            var validationResult = await validator.ValidateAsync(asset);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var existingAsset = await dbContext.Assets.FirstOrDefaultAsync(a => a.Symbol == asset.Symbol);
            if (existingAsset is null)
            {
                return Results.NotFound($"No asset found with the symbol '{asset.Symbol}'.");
            }

            existingAsset.Name = asset.Name;
            existingAsset.ISIN = asset.ISIN;

            await dbContext.SaveChangesAsync();
            return Results.Ok(asset);
        }
    }
}
