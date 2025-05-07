namespace AssetPrices.Api.Endpoints
{
    public static class PriceEndpoints
    {
        public static void MapPriceEndpoints(this WebApplication app)
        {
            var priceGroup = app.MapGroup("api/prices");

            priceGroup.MapGet("/", PriceHandlers.GetPrices);
            priceGroup.MapPost("/", PriceHandlers.CreatePrice);
            priceGroup.MapPut("/", PriceHandlers.UpdatePrice);
        }
    }
}
