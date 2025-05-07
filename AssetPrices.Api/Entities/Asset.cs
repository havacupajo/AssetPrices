namespace AssetPrices.Api.Entities
{
    public class Asset
    {
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ISIN { get; set; } = string.Empty;
    }
}
