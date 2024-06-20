using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces.Infrastructure.Repositories;
using TaxCalculator.Domain.ValueObjects;

namespace TaxCalculator.Application.Services
{
    public class HostedService : IHostedService
    {
        private readonly ITaxConfigRepository _taxConfigRepository;
        public HostedService(ITaxConfigRepository taxConfigRepository)
        {
            _taxConfigRepository = taxConfigRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeTaxConfigTable();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task InitializeTaxConfigTable()
        {
            await _taxConfigRepository.CreateTaxConfigTable();

            if (await _taxConfigRepository.CheckTaxConfigTableCount() == 0)
            {
                await _taxConfigRepository.InsertDefaultValuesToTable();
            }
        }
    }
}
