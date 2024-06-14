using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.ValueObjects
{
    public class Taxes
    {
        public decimal GrossIncome { get; private set; }
        public decimal CharitySpent { get; private set; }
        public Dictionary<string, decimal> AppliedTaxes { get; private set; }
        public decimal TotalTax { get; private set; }
        public decimal NetIncome { get; private set; }

        public Taxes(decimal grossIncome, decimal charitySpent, Dictionary<string, decimal> appliedTaxes, decimal totalTax, decimal netIncome)
        {
            GrossIncome = grossIncome;
            CharitySpent = charitySpent;
            AppliedTaxes = appliedTaxes;
            TotalTax = totalTax;
            NetIncome = netIncome;
        }
    }
}
