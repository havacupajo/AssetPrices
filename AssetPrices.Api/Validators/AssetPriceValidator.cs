using AssetPrices.Api.Contracts;
using FluentValidation;

namespace AssetPrices.Api.Validators
{
    public class AssetPriceValidator : AbstractValidator<AssetPriceDto>
    {
        public AssetPriceValidator()
        {
            RuleFor(p => p.Symbol).NotEmpty().WithMessage("Symbol is required.");
            RuleFor(p => p.Source).NotEmpty().WithMessage("Source is required.");
            RuleFor(p => p.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}
