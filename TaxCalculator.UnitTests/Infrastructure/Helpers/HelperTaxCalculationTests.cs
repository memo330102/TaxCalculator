using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.ValueObjects;
using TaxCalculator.Infrastructure.Helpers;
using Xunit;

namespace TaxCalculator.UnitTests.Infrastructure.Helpers
{
    public class HelperTaxCalculationTests
    {
        private readonly Mock<ISqlQuery> _mockSqlQuery;
        private readonly HelperTaxCalculation _helperTaxCalculation;

        public HelperTaxCalculationTests()
        {
            _mockSqlQuery = new Mock<ISqlQuery>();
            _helperTaxCalculation = new HelperTaxCalculation(_mockSqlQuery.Object);

            var taxConfig = new TaxConfig
            {
                MinApplyableIncomeTax = 1000,
                CharitySpentMaxRate = 0.10m
            };

            _mockSqlQuery
                .Setup(sq => sq.QueryAsyncFirstOrDefault<TaxConfig>(It.IsAny<string>(),It.IsAny<object>()))
                .ReturnsAsync(taxConfig);
        }

        [Fact]
        public async Task HelperTaxCalculation_TaxableIncome_With_Income_Higher_Than_Threshold_Should_Return_TaxableIncome()
        {
            decimal grossIncome = 2000;

            var taxableIncome = await _helperTaxCalculation.TaxableIncome(grossIncome);

            Assert.Equal(1000, taxableIncome);
        }

        [Theory]
        [InlineData(1000, 0)]
        [InlineData(700, 0)]
        public async Task HelperTaxCalculation_TaxableIncome_With_Income_Less_Than_Or_Equal_Threshold_Should_Return_Zero(decimal grossIncome, decimal expected)
        {

            var taxableIncome = await _helperTaxCalculation.TaxableIncome(grossIncome);

            Assert.Equal(expected, taxableIncome);
        }

        [Theory]
        [InlineData(2000, 150,150)]
        [InlineData(3000,300,300)]
        public async Task HelperTaxCalculation_CharityAdjustment_With_CharitySpent_Should_Return_Adjustment(decimal grossIncome, decimal charitySpent, decimal expected)
        {

            var charityAdjustment = await _helperTaxCalculation.CharityAdjustment(grossIncome, charitySpent);

            Assert.Equal(expected, charityAdjustment);
        }

        [Theory]
        [InlineData(2000, 150, 150)]
        [InlineData(2000, 250, 200)]
        public async Task HelperTaxCalculation_CharityAdjustment_WithCharitySpentAboveMaxRate_ShouldReturnMaxRateAdjustment(decimal grossIncome, decimal charitySpent, decimal expected)
        {

            var charityAdjustment = await _helperTaxCalculation.CharityAdjustment(grossIncome, charitySpent);

            Assert.Equal(expected, charityAdjustment);
        }
    }
}
