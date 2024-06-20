using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Domain.Interfaces.Infrastructure.Repositories
{
    public interface ITaxConfigRepository
    {
        public Task<TaxConfig> GetTaxConfigAsync();
        public Task<int> CheckTaxConfigTableCount();
        public Task InsertDefaultValuesToTable();
        public Task CreateTaxConfigTable();

    }
}
