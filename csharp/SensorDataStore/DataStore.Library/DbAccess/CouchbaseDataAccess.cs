using Core.Library.Models;
using System;
using System.Collections.Generic;

namespace DataStore.Library.DbAccess
{
    public interface ICouchbaseDataAccess
    {
        IEnumerable<BmpMeasurementDto> LoadData();
    }

    public class CouchbaseDataAccess : ICouchbaseDataAccess
    {
        public IEnumerable<BmpMeasurementDto> LoadData()
        {
            return GenerateMeasurement();
        }

        private IEnumerable<BmpMeasurementDto> GenerateMeasurement()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new BmpMeasurementDto
                {
                    DateTime = DateTime.Now,
                    Pressure = 1000 + i,
                    Temperature = 20 + i
                };
            }
        }
    }
}
