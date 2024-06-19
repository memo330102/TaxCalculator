using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Interfaces;

namespace TaxCalculator.Infrastructure.Services
{
    public class IncomeTaxCalculator : ITaxCalculator
    {
        private IHelperTaxCalculation _helperTaxCalculation;
        public IncomeTaxCalculator(IHelperTaxCalculation helperTaxCalculation)
        {
            _helperTaxCalculation = helperTaxCalculation;
        }
        public string TaxType => TaxTypeEnum.IncomeTax.ToString();

        public async Task<decimal> CalculateTax(TaxPayer taxPayer)
        {
            try
            {
                var taxConfig = await _helperTaxCalculation.GetTaxConfigAsync();

                decimal taxableIncome = await _helperTaxCalculation.TaxableIncome(taxPayer.GrossIncome);

                decimal charityAdjustment = await _helperTaxCalculation.CharityAdjustment(taxPayer.GrossIncome, taxPayer.CharitySpent);

                taxableIncome = await _helperTaxCalculation.AdjustTaxableIncome(taxableIncome, charityAdjustment);

                return taxableIncome * taxConfig.IncomeTaxRate;
            }
            catch (ArgumentNullException ex)
            {
                Log.Error(ex, "Income Tax Error ArgumentNullException ", ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, "Income Tax Error InvalidOperationException ", ex.Message);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(ex, "Income Tax Error KeyNotFoundException ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Income Tax Error ", ex.Message);
                throw;
            }
        }
    }
}
