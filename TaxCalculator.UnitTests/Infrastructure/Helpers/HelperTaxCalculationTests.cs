using Moq;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces.Infrastructure.Repositories;
using TaxCalculator.Domain.ValueObjects;
using TaxCalculator.Infrastructure.Helpers;
using Xunit;

namespace TaxCalculator.UnitTests.Infrastructure.Helpers
{
    public class HelperTaxCalculationTests
    {
        private readonly Mock<ITaxConfigRepository> _mockTaxConfigRepository;
        private readonly HelperTaxCalculation _helperTaxCalculation;

        public HelperTaxCalculationTests()
        {
            _mockTaxConfigRepository = new Mock<ITaxConfigRepository>();
            _helperTaxCalculation = new HelperTaxCalculation(_mockTaxConfigRepository.Object);

            var taxConfig = new TaxConfig
            {
                MinApplyableIncomeTax = 1000m,
                CharitySpentMaxRate = 0.10m,
                IncomeTaxRate = 0.10m,
                SocialTaxRate = 0.15m,
                MinApplyableSocialTax = 1000m,
                MaxApplyableSocialTax = 3000m
            };

            _mockTaxConfigRepository
                .Setup(sq => sq.GetTaxConfigAsync())
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
        [InlineData(2000, 150, 150)]
        [InlineData(3000, 300, 300)]
        public async Task HelperTaxCalculation_CharityAdjustment_With_CharitySpent_Should_Return_Adjustment(decimal grossIncome, decimal charitySpent, decimal expected)
        {

            var charityAdjustment = await _helperTaxCalculation.CharityAdjustment(grossIncome, charitySpent);

            Assert.Equal(expected, charityAdjustment);
        }

        [Theory]
        [InlineData(2500, 500, 250)]
        [InlineData(2000, 250, 200)]
        public async Task HelperTaxCalculation_CharityAdjustment_With_CharitySpent_Higher_than_MaxRate_Should_Return_MaxRate_Adjustment(decimal grossIncome, decimal charitySpent, decimal expected)
        {

            var charityAdjustment = await _helperTaxCalculation.CharityAdjustment(grossIncome, charitySpent);

            Assert.Equal(expected, charityAdjustment);
        }

        [Fact]
        public async Task HelperTaxCalculation_AdjustTaxableIncome_With_Positive_Income_And_Charity_Should_Return_Adjusted_Income()
        {
            decimal taxableIncome = 1000;
            decimal charityAdjustment = 200;

            var adjustedIncome = await _helperTaxCalculation.AdjustTaxableIncome(taxableIncome, charityAdjustment);

            Assert.Equal(800, adjustedIncome);
        }

        [Fact]
        public async Task HelperTaxCalculation_AdjustTaxableIncome_With_Negative_Result_Should_Return_Zero()
        {
            decimal taxableIncome = 100;
            decimal charityAdjustment = 200;

            var adjustedIncome = await _helperTaxCalculation.AdjustTaxableIncome(taxableIncome, charityAdjustment);

            Assert.Equal(0, adjustedIncome);
        }
    }
}
