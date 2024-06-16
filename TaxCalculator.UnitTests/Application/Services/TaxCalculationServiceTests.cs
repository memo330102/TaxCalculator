using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Application.Services;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Interfaces;
using Xunit;

namespace TaxCalculator.UnitTests.Application.Services
{
    public class TaxCalculationServiceTests
    {
        private readonly Mock<ITaxCalculator> _incomeTaxCalculatorMock;
        private readonly Mock<ITaxCalculator> _socialTaxCalculatorMock;
        private readonly TaxCalculationService _taxCalculationService;
        public TaxCalculationServiceTests()
        {
            _incomeTaxCalculatorMock = new Mock<ITaxCalculator>();
            _socialTaxCalculatorMock = new Mock<ITaxCalculator>();

            var taxCalculators = new List<ITaxCalculator>
            {
                _incomeTaxCalculatorMock.Object,
                _socialTaxCalculatorMock.Object
            };

            _taxCalculationService = new TaxCalculationService(taxCalculators);

        }

        [Fact]
        public async Task TaxCalculationService_CalculateTaxes_With_Valid_TaxPayer_Should_Return_Correct_Taxes()
        {
            var taxPayer = new TaxPayer("Mehmet Aksak", DateTime.Now, 5000, 1234567890, 100);

            _incomeTaxCalculatorMock.Setup(tc => tc.CalculateTax(It.IsAny<TaxPayer>())).ReturnsAsync(390);
            _incomeTaxCalculatorMock.Setup(tc => tc.TaxType).Returns(TaxTypeEnum.IncomeTax.ToString());

            _socialTaxCalculatorMock.Setup(tc => tc.CalculateTax(It.IsAny<TaxPayer>())).ReturnsAsync(300);
            _socialTaxCalculatorMock.Setup(tc => tc.TaxType).Returns(TaxTypeEnum.SocialTax.ToString());

            var result = await _taxCalculationService.CalculateTaxes(taxPayer);

            Assert.Equal(5000, result.GrossIncome);
            Assert.Equal(100, result.CharitySpent);
            Assert.Equal(690, result.TotalTax);
            Assert.Equal(4310, result.NetIncome);
            Assert.Equal(2, result.AppliedTaxes.Count);
            Assert.Equal(390, result.AppliedTaxes[TaxTypeEnum.IncomeTax.ToString()]);
            Assert.Equal(300, result.AppliedTaxes[TaxTypeEnum.SocialTax.ToString()]);
        }

        [Fact]
        public async Task CalculateTaxes_WithZeroGrossIncome_ShouldReturnZeroTaxes()
        {
            var taxPayer = new TaxPayer("Mehmet Aksak", DateTime.Now, 0, 4234355, 50);

            _incomeTaxCalculatorMock.Setup(tc => tc.CalculateTax(It.IsAny<TaxPayer>())).ReturnsAsync(0);
            _incomeTaxCalculatorMock.Setup(tc => tc.TaxType).Returns(TaxTypeEnum.IncomeTax.ToString());

            _socialTaxCalculatorMock.Setup(tc => tc.CalculateTax(It.IsAny<TaxPayer>())).ReturnsAsync(0);
            _socialTaxCalculatorMock.Setup(tc => tc.TaxType).Returns(TaxTypeEnum.SocialTax.ToString());

            var result = await _taxCalculationService.CalculateTaxes(taxPayer);

            Assert.Equal(0, result.GrossIncome);
            Assert.Equal(50, result.CharitySpent);
            Assert.Equal(0, result.TotalTax);
            Assert.Equal(0, result.NetIncome);
            Assert.Equal(2, result.AppliedTaxes.Count);
            Assert.Equal(0, result.AppliedTaxes[TaxTypeEnum.IncomeTax.ToString()]);
            Assert.Equal(0, result.AppliedTaxes[TaxTypeEnum.SocialTax.ToString()]);
        }
    }
}
