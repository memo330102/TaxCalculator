using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Infrastructure.Connection;

namespace TaxCalculator.Infrastructure.Sql.Dapper
{
    public class SqlQuery : ISqlQuery
    {
        private DbConnections _connection;

        public SqlQuery(DbConnections connection)
        {
            _connection = connection;
        }
        public async Task<int> ExecuteAsync(string query, object parameters = null)
        {
            using var con = _connection.CreateConnection();
            con.Open();
            return await con.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null)
        {
            try
            {
                using var con = _connection.CreateConnection();
                con.Open();
                return await con.QueryAsync<T>(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> QueryAsyncFirstOrDefault<T>(string query, object parameters = null)
        {
            try
            {
                using var con = _connection.CreateConnection();
                con.Open();
                return await con.QueryFirstOrDefaultAsync<T>(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
