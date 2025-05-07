namespace AssetPrices.Api.Entities
{
    public class AssetPrice
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
