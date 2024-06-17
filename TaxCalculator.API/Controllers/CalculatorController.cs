using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
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
                Log.Error("TaxPayer is null.");
                return NotFound("TaxPayer cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                Log.Warning("Model state is invalid: {ModelStateErrors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                if (TaxCache.ContainsKey(taxPayer.SSN))
                {
                    Log.Information("Tax Payer is found in cache " + taxPayer.SSN);
                    return Ok(TaxCache[taxPayer.SSN]);
                }

                var taxes = await _taxCalculationService.CalculateTaxes(taxPayer);
                Log.Information("New tax calculated. " + JsonConvert.SerializeObject(taxes));
                TaxCache[taxPayer.SSN] = taxes;

                return Ok(taxes);
            }
            catch (ArgumentException aex)
            {
                Log.Error(aex, "An argument exception occurred while calculating taxes for SSN: {SSN}", taxPayer.SSN);

                return BadRequest(aex.Message);
            }
            catch (InvalidOperationException ioex)
            {
                Log.Error(ioex, "An invalid operation occurred while calculating taxes for SSN: {SSN}", taxPayer.SSN);

                return BadRequest("An error occurred during the tax calculation process." + ioex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while calculating taxes for SSN: {SSN}", taxPayer.SSN);

                return BadRequest("An unexpected error occurred." + ex.Message);

            }
        }
    }
}
