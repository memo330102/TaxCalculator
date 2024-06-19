using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Domain.Interfaces
{
    public interface IValidator<T>
    {
        Task<ValidationResult> Validate(T entity);
    }
}
