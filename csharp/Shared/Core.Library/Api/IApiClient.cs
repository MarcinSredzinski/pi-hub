using Core.Library.Models;

namespace Core.Library.Api
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> PostSensorDataAsync(BmpMeasurementDto measurement);
    }
}
