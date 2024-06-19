using Moq;
using System;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.ValueObjects;
using TaxCalculator.Infrastructure.Services;
using Xunit;

namespace TaxCalculator.UnitTests.Infrastructure.Services
{
    public class IncomeTaxCalculatorTests
    {
        private readonly Mock<IHelperTaxCalculation> _mockHelperTaxCalculation;
        private readonly IncomeTaxCalculator _incomeTaxCalculator;
        public IncomeTaxCalculatorTests()
        {
            _mockHelperTaxCalculation = new Mock<IHelperTaxCalculation>();
            _incomeTaxCalculator = new IncomeTaxCalculator(_mockHelperTaxCalculation.Object);

            Setup();
        }

        private void Setup()
        {
            var taxConfig = new TaxConfig
            {
                IncomeTaxRate = 0.10m,
                MinApplyableIncomeTax = 1000m,
                CharitySpentMaxRate = 0.10m,
            };

            _mockHelperTaxCalculation
                .Setup(h => h.GetTaxConfigAsync())
                .ReturnsAsync(taxConfig);

            _mockHelperTaxCalculation
                .Setup(h => h.TaxableIncome(It.IsAny<decimal>()))
                .ReturnsAsync((decimal grossIncome) => grossIncome > taxConfig.MinApplyableIncomeTax ? grossIncome - taxConfig.MinApplyableIncomeTax : 0);

            _mockHelperTaxCalculation
                .Setup(h => h.CharityAdjustment(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ReturnsAsync((decimal grossIncome, decimal charitySpent) =>
                    Math.Min(charitySpent, grossIncome * taxConfig.CharitySpentMaxRate));

            _mockHelperTaxCalculation
                .Setup(h => h.AdjustTaxableIncome(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ReturnsAsync((decimal taxableIncome, decimal charityAdjustment) =>
                    Math.Max(taxableIncome - charityAdjustment, 0));
        }

        [Fact]
        public async Task IncomeTaxCalculator_CalculateTax_With_Less_Than_Or_Equal_1000_Income_And_No_Charity_Should_Return_IncomeTax_As_Zero()
        {
            var taxPayer = new TaxPayer("Mehmet Aksak", DateTime.Now, 500, 1234567890, 0);

            var tax = await _incomeTaxCalculator.CalculateTax(taxPayer);

            Assert.Equal(0, tax);
        }

        [Fact]
        public async Task IncomeTaxCalculator_CalculateTax_With_Higher_Than_1000_Income_And_No_Charity_Should_Return_IncomeTax()
        {
            var taxPayer = new TaxPayer("Mehmet Aksak", DateTime.Now, 1800, 1234567890, 0);

            var tax = await _incomeTaxCalculator.CalculateTax(taxPayer);

            Assert.Equal(80, tax);
        }

        [Fact]
        public async Task IncomeTaxCalculator_CalculateTax_With_Income_1000_And_No_Charity_Should_Return_IncomeTax_As_Zero()

        {
            var taxPayer = new TaxPayer("Mehmet Aksak", DateTime.Now, 1000, 1234567890, 0);

            var tax = await _incomeTaxCalculator.CalculateTax(taxPayer);

            Assert.Equal(0, tax);
        }


        [Theory]
        [InlineData(1000, 50, 0)]
        [InlineData(800, 100, 0)]
        [InlineData(0, 100, 0)]
        public async Task IncomeTaxCalculator_CalculateTax_With_Less_Than_Or_Equal_1000_Income_And_With_Charity_Should_Return_IncomeTax_As_Zero(decimal grossIncome, decimal charitySpent, decimal expectedTax)
        {
            var taxPayer = new TaxPayer("Mehmet Aksak", DateTime.Now, grossIncome, 1234567890, charitySpent);

            var tax = await _incomeTaxCalculator.CalculateTax(taxPayer);

            Assert.Equal(expectedTax, tax);
        }

        [Theory]
        [InlineData(1100, 100, 0)]
        [InlineData(1100, 110, 0)]
        [InlineData(1050, 120, 0)]
        [InlineData(1050, 300, 0)]
        [InlineData(1100, 300, 0)]
        public async Task IncomeTaxCalculator_CalculateTax_If_Taxable_Income_Less_Than_Or_Equal_1000_And_With_Higher_Than_1000_Income_And_With_Charity_Should_Return_IncomeTax_As_Zero(decimal grossIncome, decimal charitySpent, decimal expectedTax)
        {
            var taxPayer = new TaxPayer("Mehmet Aksak", DateTime.Now, grossIncome, 1234567890, charitySpent);

            var tax = await _incomeTaxCalculator.CalculateTax(taxPayer);

            Assert.Equal(expectedTax, tax);
        }

        [Theory]
        [InlineData(1200, 100, 10)]
        [InlineData(1200, 200, 8)]
        public async Task IncomeTaxCalculator_CalculateTax_If_Taxable_Income_Higher_Than_1000_And_With_Higher_Than_1000_Income_And_With_Charity_Should_Return_IncomeTax(decimal grossIncome, decimal charitySpent, decimal expectedTax)
        {
            var taxPayer = new TaxPayer("Mehmet Aksak", DateTime.Now, grossIncome, 1234567890, charitySpent);

            var tax = await _incomeTaxCalculator.CalculateTax(taxPayer);

            Assert.Equal(expectedTax, tax);
        }

        [Theory]
        [InlineData("Ricardo Meloni", 2000, 12347, 100,90)]
        [InlineData("Cahit Yigit", 1000, 12348, 0, 0)]
        [InlineData("Ihsan Yoldas", 1250, 12349, 250,12.5)]
        [InlineData("Didem Zorba", 1100, 12349, 100, 0)]
        [InlineData("Dorota Wyrobek", 1500, 12346, 50, 45)]
        public async Task IncomeTaxCalculator_CalculateTax_Should_Return_IncomeTax(string fullName, decimal grossIncome, int ssn, decimal charitySpent, decimal expectedTax)
        {
            var taxPayer = new TaxPayer(fullName, DateTime.Now, grossIncome, ssn, charitySpent);

            var tax = await _incomeTaxCalculator.CalculateTax(taxPayer);

            Assert.Equal(expectedTax, tax);
        }
    }
}
