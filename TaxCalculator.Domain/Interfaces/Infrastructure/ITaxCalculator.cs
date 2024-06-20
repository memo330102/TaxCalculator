using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Domain.Interfaces
{
    public interface ITaxCalculator
    {
        public Task<decimal> CalculateTax(TaxPayer taxPayer);
        public string TaxType { get; }
    }
}
