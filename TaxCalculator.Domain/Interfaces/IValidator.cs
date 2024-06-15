using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Interfaces
{
    public interface IValidator<T>
    {
        Task<ValidationResult> Validate(T entity);
    }
}
