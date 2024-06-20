using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Interfaces.Infrastructure.Repositories;

namespace TaxCalculator.Infrastructure.Services
{
    public class IncomeTaxCalculator : ITaxCalculator
    {
        private readonly IHelperTaxCalculation _helperTaxCalculation;
        private readonly ITaxConfigRepository _taxConfigRepository;
        public IncomeTaxCalculator(IHelperTaxCalculation helperTaxCalculation, ITaxConfigRepository taxConfigRepository)
        {
            _helperTaxCalculation = helperTaxCalculation;
            _taxConfigRepository = taxConfigRepository;
        }
        public string TaxType => TaxTypeEnum.IncomeTax.ToString();

        public async Task<decimal> CalculateTax(TaxPayer taxPayer)
        {

            var taxConfig = await _taxConfigRepository.GetTaxConfigAsync();

            decimal taxableIncome = await _helperTaxCalculation.TaxableIncome(taxPayer.GrossIncome);

            decimal charityAdjustment = await _helperTaxCalculation.CharityAdjustment(taxPayer.GrossIncome, taxPayer.CharitySpent);

            taxableIncome = await _helperTaxCalculation.AdjustTaxableIncome(taxableIncome, charityAdjustment);

            return taxableIncome * taxConfig.IncomeTaxRate;
        }
    }
}
