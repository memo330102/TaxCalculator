using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Interfaces
{
    public interface IHelperTaxCalculation
    {
        decimal TaxableIncome(decimal grossIncome);
        decimal CharityAdjustment(decimal grossIncome, decimal charitySpent);
        decimal AdjustTaxableIncome(decimal taxableIncome, decimal charityAdjustment);
    }
}
