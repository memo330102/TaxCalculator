
namespace TaxCalculator.Domain.Interfaces
{
	public interface ISqlQuery
	{
		public Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null);
		public Task<T> QueryAsyncFirstOrDefault<T>(string query, object parameters = null);
		public Task<int> ExecuteAsync(string query, object parameters = null);
	}
}
