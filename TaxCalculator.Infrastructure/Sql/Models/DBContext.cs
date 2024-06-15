using Dapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Infrastructure.Connection;

namespace TaxCalculator.Infrastructure.Sql.Models
{
    public class DBContext : IHostedService
    {
        private readonly ISqlQuery _sqlQuery;

        public DBContext(ISqlQuery sqlQuery)
        {
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
                                                         MinApplyableIncomeTax, IncomeTaxRate, CharitySpentMaxRate)
                                                 VALUES 
                                                        (@MinApplyableSocialTax, @MaxApplyableSocialTax, @SocialTaxRate,
                                                         @MinApplyableIncomeTax, @IncomeTaxRate, @CharitySpentMaxRate);";

                var defaultValues = new
                {
                    MinApplyableSocialTax = 1000m,
                    MaxApplyableSocialTax = 3000m,
                    SocialTaxRate = 0.15m,
                    MinApplyableIncomeTax = 1000m,
                    IncomeTaxRate = 0.15m,
                    CharitySpentMaxRate = 0.10m
                };

                await _sqlQuery.ExecuteAsync(insertDefaultValuesQuery, defaultValues);
            }
        }
        public async Task InitializeDatabase()
        {
            var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS TaxConfig (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MinApplyableSocialTax DECIMAL NOT NULL,
                    MaxApplyableSocialTax DECIMAL NOT NULL,
                    SocialTaxRate DECIMAL NOT NULL,
                    MinApplyableIncomeTax DECIMAL NOT NULL,
                    IncomeTaxRate DECIMAL NOT NULL,
                    CharitySpentMaxRate DECIMAL NOT NULL
                );
            ";

            await _sqlQuery.ExecuteAsync(createTableQuery);

            await insertDefaultValuesToTable();
        }
    }
}
