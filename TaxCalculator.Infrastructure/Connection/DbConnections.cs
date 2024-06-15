using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Infrastructure.Connection
{
    public class DbConnections
    {
        private IConfiguration _configuration;

        public DbConnections(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            string connectionString = _configuration.GetConnectionString("DBTaxCalculator");

            return new SqliteConnection(connectionString);
        }
    }
}
