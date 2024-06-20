using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Interfaces.Infrastructure.Repositories;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Infrastructure.Repositories
{
    public class TaxConfigRepository : ITaxConfigRepository
    {
        private readonly ISqlQuery _sqlQuery;
        public TaxConfigRepository(ISqlQuery sqlQuery)
        {
            _sqlQuery = sqlQuery;
        }
        public async Task<TaxConfig> GetTaxConfigAsync()
        {
            var query = @"SELECT * FROM TaxConfig LIMIT 1;";
            return await _sqlQuery.QueryAsyncFirstOrDefault<TaxConfig>(query);
        }
    }
}
