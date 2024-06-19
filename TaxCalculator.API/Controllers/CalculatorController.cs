using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using TaxCalculator.Application.Services;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ITaxCalculationService _taxCalculationService;
        private readonly IMemoryCache _memoryCache;
        public CalculatorController(ITaxCalculationService taxCalculationService, IMemoryCache memoryCache)
        {
            _taxCalculationService = taxCalculationService;
            _memoryCache = memoryCache;
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

            if (_memoryCache.TryGetValue(taxPayer.SSN, out Taxes cachedTaxes))
            {
                Log.Information("Tax Payer found in cache: " + taxPayer.SSN);
                return Ok(cachedTaxes);
            }

            var taxes = await _taxCalculationService.CalculateTaxes(taxPayer);
            Log.Information("New tax calculated. " + JsonConvert.SerializeObject(taxes));

            _memoryCache.Set(taxPayer.SSN, taxes, TimeSpan.FromDays(1));

            return Ok(taxes);
        }
    }
}
