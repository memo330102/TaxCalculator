using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Application.Services
{
    public interface ITaxCalculationService
    {
        Task<Taxes> CalculateTaxes(TaxPayer taxPayer);
    }
}
