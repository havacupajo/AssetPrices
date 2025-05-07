namespace AssetPrices.Api.Endpoints
{
    public static class AssetEndpoints
    {
        public static void MapAssetEndpoints(this WebApplication app)
        {
            var assetGroup = app.MapGroup("api/assets");

            assetGroup.MapGet("/", AssetHandlers.GetAllAssets);
            assetGroup.MapGet("/{symbol}", AssetHandlers.GetAssetBySymbol);
            assetGroup.MapPost("/", AssetHandlers.CreateAsset);
            assetGroup.MapPut("/", AssetHandlers.UpdateAsset);
        }
    }
}
