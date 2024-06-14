using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Interfaces;

namespace TaxCalculator.Infrastructure.Services
{
    public class SocialTaxCalculator : ITaxCalculator
    {
        private IHelperTaxCalculation _helperTaxCalculation;
        public SocialTaxCalculator(IHelperTaxCalculation helperTaxCalculation)
        {
            _helperTaxCalculation = helperTaxCalculation;
        }
        public string TaxType => TaxTypeEnum.SocialTax.ToString();

        public decimal CalculateTax(TaxPayer taxPayer)
        {
            decimal taxableIncome = _helperTaxCalculation.TaxableIncome(taxPayer.GrossIncome);

            decimal charityAdjustment = _helperTaxCalculation.CharityAdjustment(taxPayer.GrossIncome, taxPayer.CharitySpent);

            taxableIncome = _helperTaxCalculation.AdjustTaxableIncome(taxableIncome, charityAdjustment);

            decimal socialTaxableIncome = taxPayer.GrossIncome > 1000 ? Math.Min(taxableIncome, 3000 - 1000) : 0;

            return socialTaxableIncome * 0.15m;
        }
    }
}
