namespace AssetPrices.Api.Contracts
{
    public class AssetPriceDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public decimal Price { get; set; }
    }
}
