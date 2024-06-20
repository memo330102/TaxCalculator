using Microsoft.Extensions.Options;
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
        private readonly TaxConfig _taxConfig;
        public TaxConfigRepository(IOptions<TaxConfig> taxConfigOptions, ISqlQuery sqlQuery)
        {
            _sqlQuery = sqlQuery;
            _taxConfig = taxConfigOptions.Value;
        }

        public async Task<int> CheckTaxConfigTableCount()
        {
            var checkIfEmptyQuery = @"SELECT COUNT(1) FROM TaxConfig;";

            var recordCount = await _sqlQuery.QueryAsyncFirstOrDefault<int>(checkIfEmptyQuery);

            return recordCount;
        }

        public async Task CreateTaxConfigTable()
        {
            var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS TaxConfig (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MinApplyableSocialTax DECIMAL,
                    MaxApplyableSocialTax DECIMAL,
                    SocialTaxRate DECIMAL,
                    MinApplyableIncomeTax DECIMAL,
                    IncomeTaxRate DECIMAL ,
                    CharitySpentMaxRate DECIMAL,
                    BaseCurrency text 
                );
            ";

            await _sqlQuery.ExecuteAsync(createTableQuery);
        }

        public async Task<TaxConfig> GetTaxConfigAsync()
        {
            var query = @"SELECT * FROM TaxConfig LIMIT 1;";
            return await _sqlQuery.QueryAsyncFirstOrDefault<TaxConfig>(query);
        }

        public async Task InsertDefaultValuesToTable()
        {
            var insertDefaultValuesQuery = @"INSERT INTO TaxConfig 
                                                 (MinApplyableSocialTax, MaxApplyableSocialTax, SocialTaxRate,
                                                  MinApplyableIncomeTax, IncomeTaxRate, CharitySpentMaxRate,BaseCurrency)
                                             VALUES 
                                                  (@MinApplyableSocialTax, @MaxApplyableSocialTax, @SocialTaxRate,
                                                   @MinApplyableIncomeTax, @IncomeTaxRate, @CharitySpentMaxRate,@BaseCurrency);";

            var defaultValues = new
            {
                MinApplyableSocialTax = _taxConfig.MinApplyableSocialTax,
                MaxApplyableSocialTax = _taxConfig.MaxApplyableSocialTax,
                SocialTaxRate = _taxConfig.SocialTaxRate,
                MinApplyableIncomeTax = _taxConfig.MinApplyableIncomeTax,
                IncomeTaxRate = _taxConfig.IncomeTaxRate,
                CharitySpentMaxRate = _taxConfig.CharitySpentMaxRate,
                BaseCurrency = _taxConfig.BaseCurrency
            };

            await _sqlQuery.ExecuteAsync(insertDefaultValuesQuery, defaultValues);
        }
    }
}
