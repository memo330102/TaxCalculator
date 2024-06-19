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
    public class SocialTaxCalculator : ITaxCalculator
    {
        private IHelperTaxCalculation _helperTaxCalculation;
        public SocialTaxCalculator(IHelperTaxCalculation helperTaxCalculation)
        {
            _helperTaxCalculation = helperTaxCalculation;
        }
        public string TaxType => TaxTypeEnum.SocialTax.ToString();

        public async Task<decimal> CalculateTax(TaxPayer taxPayer)
        {
            try
            {
                var taxConfig = await _helperTaxCalculation.GetTaxConfigAsync();

                decimal taxableIncome = await _helperTaxCalculation.TaxableIncome(taxPayer.GrossIncome);

                decimal charityAdjustment = await _helperTaxCalculation.CharityAdjustment(taxPayer.GrossIncome, taxPayer.CharitySpent);

                taxableIncome = await _helperTaxCalculation.AdjustTaxableIncome(taxableIncome, charityAdjustment);

                decimal socialTaxableIncome = taxPayer.GrossIncome > taxConfig.MinApplyableSocialTax ? Math.Min(taxableIncome, taxConfig.MaxApplyableSocialTax - taxConfig.MinApplyableSocialTax) : 0;

                return socialTaxableIncome * taxConfig.SocialTaxRate;
            }
            catch (ArgumentNullException ex)
            {
                Log.Error(ex, "Social Tax Error ArgumentNullException ", ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, "Social Tax Error InvalidOperationException ", ex.Message);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(ex, "Social Tax Error KeyNotFoundException ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Social Tax Error Exception ", ex.Message);
                throw;
            }
        }
    }
}
