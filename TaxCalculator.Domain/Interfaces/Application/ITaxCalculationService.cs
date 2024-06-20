using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Domain.Interfaces.Application
{
    public interface ITaxCalculationService
    {
        Task<Taxes> CalculateTaxes(TaxPayer taxPayer);
    }
}
