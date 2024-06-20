using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Interfaces.Infrastructure.Repositories;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Infrastructure.Helpers
{
    public class HelperTaxCalculation : IHelperTaxCalculation
    {
        private readonly ITaxConfigRepository _taxConfigRepository;
        public HelperTaxCalculation(ITaxConfigRepository taxConfigRepository)
        {
            _taxConfigRepository = taxConfigRepository;
        }
        public async Task<decimal> TaxableIncome(decimal grossIncome)
        {
            var taxConfig = await _taxConfigRepository.GetTaxConfigAsync();

            return grossIncome > taxConfig.MinApplyableIncomeTax ? grossIncome - taxConfig.MinApplyableIncomeTax : 0;
        }

        public async Task<decimal> CharityAdjustment(decimal grossIncome, decimal charitySpent)
        {
            var taxConfig = await _taxConfigRepository.GetTaxConfigAsync();

            return Math.Min(charitySpent, grossIncome * taxConfig.CharitySpentMaxRate);
        }

        public async Task<decimal> AdjustTaxableIncome(decimal taxableIncome, decimal charityAdjustment)
        {
            var adjustedIncome = taxableIncome - charityAdjustment;

            return Math.Max(adjustedIncome, 0);
        }
    }
}
