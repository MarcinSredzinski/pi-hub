using Core.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Library.Abstractions
{
    public interface IBmpMeasurementsData
    {
        Task<IEnumerable<BmpMeasurementDto>> GetMeasurementsAsync();
        Task InsertMeasurementAsync(BmpMeasurementDto measurement);
    }
}
