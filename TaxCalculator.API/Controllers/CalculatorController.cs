using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Interfaces.Application;
using TaxCalculator.Domain.Interfaces.Caching;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ITaxCalculationService _taxCalculationService;
        private readonly ICachingService _cachingService;
        public CalculatorController(ITaxCalculationService taxCalculationService, ICachingService cachingService)
        {
            _taxCalculationService = taxCalculationService;
            _cachingService = cachingService;
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

            string cacheKey = taxPayer.SSN.ToString();
            if (await _cachingService.ExistsAsync(cacheKey))
            {
                Log.Information("Tax Payer found in cache:{taxPayer.SSN} " + taxPayer.SSN);
                var cachedTaxes = await _cachingService.GetAsync<Taxes>(cacheKey);
                return Ok(cachedTaxes);
            }

            var taxes = await _taxCalculationService.CalculateTaxes(taxPayer);
            Log.Information("New tax calculated. " + JsonConvert.SerializeObject(taxes));

            await _cachingService.SetAsync(cacheKey, taxes, TimeSpan.FromDays(1));

            return Ok(taxes);
        }
    }
}
