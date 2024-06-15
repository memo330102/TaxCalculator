using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;

namespace TaxCalculator.Domain.Entities
{
    public class TaxPayer 
    {
        [Required(ErrorMessage = "Full name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name can only contain letters and spaces")]
        [CustomValidation(typeof(TaxPayer), nameof(ValidateFullName))]
        public string FullName { get; private set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; private set; }

        [Required(ErrorMessage = "Gross income is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Gross income must be a valid positive number")]
        public decimal GrossIncome { get; private set; }

        [Required(ErrorMessage = "SSN is required")]
        [RegularExpression(@"^\d{5,10}$", ErrorMessage = "SSN must be a valid 5 to 10 digits number")]
        public int SSN { get; private set; }

        [Range(0, double.MaxValue, ErrorMessage = "Charity spent must be a valid positive number")]
        public decimal CharitySpent { get; private set; }

        public TaxPayer(string fullName, DateTime dateOfBirth, decimal grossIncome, int sSN, decimal charitySpent)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            GrossIncome = grossIncome;
            SSN = sSN;
            CharitySpent = charitySpent;
        }
        public static ValidationResult ValidateFullName(string fullName)
        {
            if (!string.IsNullOrWhiteSpace(fullName) && fullName.Trim().Split(' ').Length >= 2)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Full name must contain at least two words");
        }
    }
}
