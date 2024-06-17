using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.ValueObjects;
using TaxCalculator.Infrastructure.Connection;

namespace TaxCalculator.Infrastructure.Sql.Models
{
    public class DBContext : IHostedService
    {
        private readonly ISqlQuery _sqlQuery;
        private readonly TaxConfig _taxConfig;
        public DBContext(IOptions<TaxConfig> taxConfigOptions, ISqlQuery sqlQuery)
        {
            _taxConfig = taxConfigOptions.Value;
            _sqlQuery = sqlQuery;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeDatabase();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task InitializeDatabase()
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

            await insertDefaultValuesToTable();
        }


        public async Task<int> CheckTagConfigTableCount()
        {
            var checkIfEmptyQuery = @"SELECT COUNT(1) FROM TaxConfig;";

            var recordCount = await _sqlQuery.QueryAsyncFirstOrDefault<int>(checkIfEmptyQuery);
            return recordCount;
        }

        public async Task insertDefaultValuesToTable()
        {
            if (await CheckTagConfigTableCount() == 0)
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
                    BaseCurrency= _taxConfig.BaseCurrency
                };

                await _sqlQuery.ExecuteAsync(insertDefaultValuesQuery, defaultValues);
            }
        }
    }
}
