using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.ValueObjects;
using TaxCalculator.Infrastructure.Services;

namespace TaxCalculator.Application.Services
{
    public class TaxCalculationService
    {
        private readonly IEnumerable<ITaxCalculator> _taxCalculators;

        public TaxCalculationService(IEnumerable<ITaxCalculator> taxCalculators)
        {
            _taxCalculators = taxCalculators;
        }

        public async Task<Taxes> CalculateTaxes(TaxPayer taxPayer)
        {
            var appliedTaxes = new Dictionary<string, decimal>();

            foreach (ITaxCalculator taxCalculator in _taxCalculators)
            {
                appliedTaxes[taxCalculator.TaxType] = await taxCalculator.CalculateTax(taxPayer);
            }

            decimal totalTax = appliedTaxes.Values.Sum()
                ;
            decimal netIncome = taxPayer.GrossIncome - totalTax;



            return new Taxes(taxPayer.GrossIncome, taxPayer.CharitySpent, appliedTaxes, totalTax, netIncome);
        }
    }
}
