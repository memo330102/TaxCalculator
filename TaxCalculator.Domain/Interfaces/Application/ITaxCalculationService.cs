using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Domain.Interfaces.Application
{
    public interface ITaxCalculationService
    {
        Task<Taxes> CalculateTaxes(TaxPayer taxPayer);
    }
}
