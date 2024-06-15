using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.ValueObjects
{
    public class TaxConfig
    {
        public decimal MinApplyableSocialTax { get; set; }
        public decimal MaxApplyableSocialTax { get; set; }
        public decimal SocialTaxRate { get; set; }
        public decimal MinApplyableIncomeTax { get; set; }
        public decimal IncomeTaxRate { get; set; }
        public decimal CharitySpentMaxRate { get; set; }
    }
}
