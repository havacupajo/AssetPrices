using AssetPrices.Api.Contracts;
using AssetPrices.Api.Database;
using AssetPrices.Api.Endpoints;
using AssetPrices.Api.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AssetPrices.Tests
{
    public class PriceHandlersTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreatePrice_ShouldReturnCreated_WhenValid()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var validatorMock = new Mock<IValidator<AssetPriceDto>>();
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<AssetPriceDto>(), default))
                .ReturnsAsync(new ValidationResult());

            var priceDto = new AssetPriceDto
            {
                Symbol = "AAPL",
                Source = "NASDAQ",
                Date = new DateOnly(2025, 5, 7),
                Price = 150.25m
            };

            // Act
            var result = await PriceHandlers.CreatePrice(priceDto, validatorMock.Object, dbContext);

            // Assert
            Assert.NotNull(result);
            Assert.True(result is Created<AssetPriceDto>);
            Assert.Equal(StatusCodes.Status201Created, (result as Created<AssetPriceDto>)?.StatusCode);
        }

        [Fact]
        public async Task UpdatePrice_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.AssetPrices.Add(new AssetPrice
            {
                Symbol = "AAPL",
                Source = "NASDAQ",
                Date = new DateOnly(2025, 5, 7),
                Price = 150.25m,
                LastUpdated = DateTime.UtcNow
            });
            await dbContext.SaveChangesAsync();

            var validatorMock = new Mock<IValidator<AssetPriceDto>>();
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<AssetPriceDto>(), default))
                .ReturnsAsync(new ValidationResult());

            var priceDto = new AssetPriceDto
            {
                Symbol = "AAPL",
                Source = "NASDAQ",
                Date = new DateOnly(2025, 5, 7),
                Price = 200.50m
            };

            // Act
            var result = await PriceHandlers.UpdatePrice(priceDto, validatorMock.Object, dbContext);

            // Assert
            Assert.NotNull(result);
            Assert.True(result is Ok<AssetPriceDto>);
            Assert.Equal(StatusCodes.Status200OK, (result as Ok<AssetPriceDto>)?.StatusCode);
        }
    }
}
