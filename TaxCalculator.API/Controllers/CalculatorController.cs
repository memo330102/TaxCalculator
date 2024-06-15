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
        private readonly ITaxCalculationService _taxCalculationService;
        private static readonly Dictionary<int, Taxes> TaxCache = new();
        public CalculatorController(ITaxCalculationService taxCalculationService)
        {
            _taxCalculationService = taxCalculationService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<Taxes>> Calculate([FromBody] TaxPayer taxPayer)
        {
            if (taxPayer == null)
            {
                //_logger.LogWarning("TaxPayer is null.");
                return NotFound("TaxPayer cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                //_logger.LogWarning("Model state is invalid: {ModelStateErrors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                if (TaxCache.ContainsKey(taxPayer.SSN))
                {
                    return Ok(TaxCache[taxPayer.SSN]);
                }

                var taxes = await _taxCalculationService.CalculateTaxes(taxPayer);

                TaxCache[taxPayer.SSN] = taxes;

                return Ok(taxes);
            }
            catch (ArgumentException aex)
            {
                //_logger.LogError(ex, "An argument exception occurred while calculating taxes for SSN: {SSN}", taxPayer.SSN);
                return BadRequest(aex.Message);
            }
            catch (InvalidOperationException ioex)
            {
                //_logger.LogError(ex, "An invalid operation occurred while calculating taxes for SSN: {SSN}", taxPayer.SSN);
                return BadRequest("An error occurred during the tax calculation process." + ioex.Message);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected error occurred while calculating taxes for SSN: {SSN}", taxPayer.SSN);
                //return StatusCode(500, "An unexpected error occurred. Please try again later.");
                return BadRequest("An unexpected error occurred." + ex.Message);

            }
        }
    }
}
