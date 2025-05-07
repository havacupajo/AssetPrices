using AssetPrices.Api.Entities;

namespace AssetPrices.Api.Database
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext dbContext, bool seedAssets)
        {
            if (seedAssets)
            {
                if (!dbContext.Assets.Any())
                {
                    dbContext.Assets.AddRange(
                        new Asset { Symbol = "AAPL", Name = "Apple Inc.", ISIN = " US0378331005" },
                        new Asset { Symbol = "MSFT", Name = "Microsoft Corporation", ISIN = " US5949181045 " },
                        new Asset { Symbol = "NVDA", Name = "NVIDIA Corporation", ISIN = " US67066G1040 " },
                        new Asset { Symbol = "AMZN", Name = "Amazon.com, Inc.", ISIN = " US0231351067 " },
                        new Asset { Symbol = "GOOGL", Name = "Alphabet Inc. (Google", ISIN = " US02079K3059 " },
                        new Asset { Symbol = "META", Name = "Meta Platforms, Inc.", ISIN = " US30303M1027 " },
                        new Asset { Symbol = "BRK.B", Name = "Berkshire Hathaway Inc.", ISIN = " US0846707026 " },
                        new Asset { Symbol = "TSLA", Name = "Tesla, Inc.", ISIN = " US88160R1014 " },
                        new Asset { Symbol = "TSM", Name = "Taiwan Semiconductor Mfg.", ISIN = " US8740391003 " },
                        new Asset { Symbol = "V", Name = "Visa Inc.", ISIN = " US92826C8394 " }
                    );
                }
                dbContext.SaveChanges();
            }
        }
    }
}
