﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Interfaces.Caching
{
    public interface ICachingService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task<bool> ExistsAsync(string key);
        Task RemoveAsync(string key);
    }
}
