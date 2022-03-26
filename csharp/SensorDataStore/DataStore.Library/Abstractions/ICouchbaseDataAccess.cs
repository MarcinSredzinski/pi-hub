using Couchbase.KeyValue;
using Couchbase.Query;
using System.Threading.Tasks;

namespace DataStore.Library.Abstractions
{
    public interface ICouchbaseDataAccess
    {
        Task<IQueryResult<T>> LoadDataAsync<T>(string query);
        ICouchbaseCollection BMPSensorData { get; }
        ICouchbaseCollection APIUsersData { get; }
    }

}
