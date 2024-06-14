using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;

namespace TaxCalculator.Infrastructure.Helpers
{
    public class HelperTaxCalculation : IHelperTaxCalculation
    {

        public decimal TaxableIncome(decimal grossIncome)
        {
            return grossIncome > 1000 ? grossIncome - 1000 : 0;
        }

        public decimal CharityAdjustment(decimal grossIncome, decimal charitySpent)
        {
            return Math.Min(charitySpent, grossIncome * 0.10m);
        }

        decimal IHelperTaxCalculation.AdjustTaxableIncome(decimal taxableIncome, decimal charityAdjustment)
        {
            var adjustedIncome = taxableIncome - charityAdjustment;

            return Math.Max(adjustedIncome, 0);
        }
    }
}
