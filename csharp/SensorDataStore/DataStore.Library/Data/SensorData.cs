using Core.Library.Models;
using Couchbase.KeyValue;
using DataStore.Library.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStore.Library.Data
{
    public class SensorData : ISensorData
    {
        private readonly ICouchbaseDataAccess _couchbaseDataAccess;

        public SensorData(ICouchbaseDataAccess couchbaseDataAccess)
        {
            _couchbaseDataAccess = couchbaseDataAccess;
        }

        public async Task<IEnumerable<BmpMeasurementDto>> GetAsync()
        {
            var all = await _couchbaseDataAccess.LoadDataAsync<dynamic>
                ("select BMPSensorData from MinimalApiUserDb._default.BMPSensorData");

            return all.Rows
                .Select(row => new BmpMeasurementDto()
                {
                    DateTime = row.BMPSensorData.DateTime ?? row.BMPSensorData.dateTime,
                    Pressure = row.BMPSensorData.Pressure ?? row.BMPSensorData.pressure,
                    Temperature = row.BMPSensorData.Temperature ?? row.BMPSensorData.temperature
                }).ToEnumerable();
        }

        public Task InsertAsync(BmpMeasurementDto measurement)
        {
            return _couchbaseDataAccess.BMPSensorData.InsertAsync(Guid.NewGuid().ToString(), measurement,
                new InsertOptions());
        }
    }
}
