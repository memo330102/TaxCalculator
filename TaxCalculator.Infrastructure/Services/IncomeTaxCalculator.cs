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
    public class IncomeTaxCalculator : ITaxCalculator
    {
        private IHelperTaxCalculation _helperTaxCalculation;
        public IncomeTaxCalculator(IHelperTaxCalculation helperTaxCalculation)
        {
            _helperTaxCalculation = helperTaxCalculation;
        }
        public string TaxType => TaxTypeEnum.IncomeTax.ToString();

        public decimal CalculateTax(TaxPayer taxPayer)
        {
            decimal taxableIncome = _helperTaxCalculation.TaxableIncome(taxPayer.GrossIncome);

            decimal charityAdjustment = _helperTaxCalculation.CharityAdjustment(taxPayer.GrossIncome, taxPayer.CharitySpent);

            taxableIncome = _helperTaxCalculation.AdjustTaxableIncome(taxableIncome, charityAdjustment);

            return taxableIncome * 0.10m;
        }
    }
}
