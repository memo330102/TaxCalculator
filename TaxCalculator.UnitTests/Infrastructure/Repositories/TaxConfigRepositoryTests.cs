using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.ValueObjects;
using TaxCalculator.Infrastructure.Repositories;
using Xunit;

namespace TaxCalculator.UnitTests.Infrastructure.Repositories
{
    public class TaxConfigRepositoryTests
    {
        private readonly Mock<ISqlQuery> _mockSqlQuery;
        private readonly IOptions<TaxConfig> _mockTaxConfigOptions;
        private readonly TaxConfigRepository _repository;

        public TaxConfigRepositoryTests()
        {
            _mockSqlQuery = new Mock<ISqlQuery>();
            var taxConfig = new TaxConfig
            {
                MinApplyableSocialTax = 1000m,
                MaxApplyableSocialTax = 5000m,
                SocialTaxRate = 0.15m,
                MinApplyableIncomeTax = 2000m,
                IncomeTaxRate = 0.25m,
                CharitySpentMaxRate = 0.1m,
                BaseCurrency = "USD"
            };
            _mockTaxConfigOptions = Options.Create(taxConfig);
            _repository = new TaxConfigRepository(_mockTaxConfigOptions, _mockSqlQuery.Object);
        }

        [Fact]
        public async Task TaxConfigRepository_CheckTaxConfigTableCount_Return_Count_As_Not_Equal_Zero()
        {
            _mockSqlQuery.Setup(x => x.QueryAsyncFirstOrDefault<int>(It.IsAny<string>(), It.IsAny<object>()))
                         .ReturnsAsync(1);

            var result = await _repository.CheckTaxConfigTableCount();

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task TaxConfigRepository_GetTaxConfigAsync_Return_TaxConfig()
        {
            var expectedConfig = _mockTaxConfigOptions.Value;
            _mockSqlQuery.Setup(x => x.QueryAsyncFirstOrDefault<TaxConfig>(It.IsAny<string>(), It.IsAny<object>()))
                         .ReturnsAsync(expectedConfig);

            var result = await _repository.GetTaxConfigAsync();

            Assert.Equal(expectedConfig, result);
        }
    }
}
