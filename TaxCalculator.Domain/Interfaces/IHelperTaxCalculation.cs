using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Domain.Interfaces
{
    public interface IHelperTaxCalculation
    {
        public Task<decimal> TaxableIncome(decimal grossIncome);
        public Task<decimal> CharityAdjustment(decimal grossIncome, decimal charitySpent);
        public Task<decimal> AdjustTaxableIncome(decimal taxableIncome, decimal charityAdjustment);
        public Task<TaxConfig> GetTaxConfigAsync();
    }
}
