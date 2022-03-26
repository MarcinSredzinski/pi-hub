using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using DataStore.Library.Abstractions;

namespace DataStore.Library.DbAccess
{
    public class CouchbaseDataAccess : ICouchbaseDataAccess
    {

        public ICluster CouchbaseCluster { get; private set; }
        public IBucket MinimalApiBucket { get; private set; }
        public ICouchbaseCollection BMPSensorData { get; private set; }
        public ICouchbaseCollection APIUsersData { get; private set; }

        public CouchbaseDataAccess(IConfiguration configuration)
        {
            try
            {
                var task = Task.Run(async () =>
                {
                    string connectionString = configuration.GetSection("ConnectionStrings:CouchBase:ConnectionString").Value;
                    string pswd = configuration.GetSection("ConnectionStrings:CouchBase:Password").Value;
                    string uname = configuration.GetSection("ConnectionStrings:CouchBase:Username").Value;
                    string bucket = configuration.GetSection("ConnectionStrings:CouchBase:Bucket").Value;

                    CouchbaseCluster = await Cluster.ConnectAsync(connectionString, uname, pswd);
                    MinimalApiBucket = await CouchbaseCluster.BucketAsync(bucket);
                    var defaultScope = await MinimalApiBucket.ScopeAsync("_default");
                    BMPSensorData = await defaultScope.CollectionAsync("BMPSensorData");
                    APIUsersData = await defaultScope.CollectionAsync("APIUsersData");
                });
                task.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) => throw x);
            }
        }

        public Task<IQueryResult<T>> LoadDataAsync<T>(string query)
        {
            return CouchbaseCluster.QueryAsync<T>(query);
        }
    }
}
