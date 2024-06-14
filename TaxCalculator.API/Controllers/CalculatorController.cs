using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Application.Services;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly TaxCalculationService _taxCalculationService;
        private static readonly Dictionary<string, Taxes> TaxCache = new();
        public CalculatorController(TaxCalculationService taxCalculationService)
        {
            _taxCalculationService = taxCalculationService;
        }

        [HttpPost("calculate")]
        public ActionResult<Taxes> Calculate([FromBody] TaxPayer taxPayer)
        {
            if (taxPayer == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (TaxCache.ContainsKey(taxPayer.SSN))
            {
                return Ok(TaxCache[taxPayer.SSN]);
            }

            var taxes = _taxCalculationService.CalculateTaxes(taxPayer);

            TaxCache[taxPayer.SSN] = taxes;

            return Ok(taxes);
        }
    }
}
