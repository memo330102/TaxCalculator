using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Infrastructure.Helpers
{
    public class HelperTaxCalculation : IHelperTaxCalculation
    {
        public ISqlQuery _sqlQuery;
        public HelperTaxCalculation(ISqlQuery sqlQuery)
        {
            _sqlQuery = sqlQuery;
        }
        public async Task<decimal> TaxableIncome(decimal grossIncome)
        {
            var taxConfig = await GetTaxConfigAsync();

            return grossIncome > taxConfig.MinApplyableIncomeTax ? grossIncome - taxConfig.MinApplyableIncomeTax : 0;
        }

        public async Task<decimal> CharityAdjustment(decimal grossIncome, decimal charitySpent)
        {
            var taxConfig = await GetTaxConfigAsync();

            return Math.Min(charitySpent, grossIncome * taxConfig.CharitySpentMaxRate);
        }

        public async Task<decimal> AdjustTaxableIncome(decimal taxableIncome, decimal charityAdjustment)
        {
            var adjustedIncome = taxableIncome - charityAdjustment;

            return Math.Max(adjustedIncome, 0);
        }

        public async Task<TaxConfig> GetTaxConfigAsync()
        {
            var query = @"SELECT * FROM TaxConfig LIMIT 1;";
            return await _sqlQuery.QueryAsyncFirstOrDefault<TaxConfig>(query);
        }
    }
}
