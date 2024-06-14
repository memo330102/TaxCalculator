using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Entities
{
    public class TaxPayer
    {
        public string FullName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public decimal GrossIncome { get; private set; }
        public string SSN { get; private set; }
        public decimal CharitySpent { get; private set; }

        public TaxPayer(string fullName, DateTime dateOfBirth, decimal grossIncome, string sSN, decimal charitySpent)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            GrossIncome = grossIncome;
            SSN = sSN;
            CharitySpent = charitySpent;
        }
    }
}
