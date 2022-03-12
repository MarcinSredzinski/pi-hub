using Core.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Library.Abstractions
{
    public interface ISensorData
    {
        Task<IEnumerable<BmpMeasurementDto>> GetAsync();
        Task InsertAsync(BmpMeasurementDto measurement);
    }
}
