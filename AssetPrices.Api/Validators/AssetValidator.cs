using AssetPrices.Api.Entities;
using FluentValidation;

namespace AssetPrices.Api.Validators
{
    public class AssetValidator : AbstractValidator<Asset>
    {
        public AssetValidator()
        {
            RuleFor(a => a.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(a => a.Symbol).NotEmpty().WithMessage("Symbol is required.");
            RuleFor(a => a.ISIN).NotEmpty().WithMessage("ISIN is required.");
        }
    }
}
